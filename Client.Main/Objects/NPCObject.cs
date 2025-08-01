using Client.Main.Objects.Player;
using Client.Main.Objects.Wings;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;
using Client.Main.Content;
using Microsoft.Xna.Framework;
using Client.Main.Models;

namespace Client.Main.Objects
{
    public abstract class NPCObject : WalkerObject
    {
        protected new ILogger _logger;

        public PlayerMaskHelmObject HelmMask { get; private set; }
        public PlayerHelmObject Helm { get; private set; }
        public PlayerArmorObject Armor { get; private set; }
        public PlayerPantObject Pants { get; private set; }
        public PlayerGloveObject Gloves { get; private set; }
        public PlayerBootObject Boots { get; private set; }
        public WeaponObject Weapon1 { get; private set; }
        public WeaponObject Weapon2 { get; private set; }
        public WingObject Wings { get; private set; }

        public NPCObject()
        {
            _logger = AppLoggerFactory?.CreateLogger(GetType());
            Interactive = true;

            // Initialize body part objects and link their animations to this parent object
            HelmMask = new PlayerMaskHelmObject { LinkParentAnimation = true, Hidden = true };
            Helm = new PlayerHelmObject { LinkParentAnimation = true };
            Armor = new PlayerArmorObject { LinkParentAnimation = true };
            Pants = new PlayerPantObject { LinkParentAnimation = true };
            Gloves = new PlayerGloveObject { LinkParentAnimation = true };
            Boots = new PlayerBootObject { LinkParentAnimation = true };
            Weapon1 = new WeaponObject { };
            Weapon2 = new WeaponObject { };
            Wings = new WingObject { LinkParentAnimation = true, Hidden = true };

            Children.Add(HelmMask);
            Children.Add(Helm);
            Children.Add(Armor);
            Children.Add(Pants);
            Children.Add(Gloves);
            Children.Add(Boots);
            Children.Add(Weapon1);
            Children.Add(Weapon2);
            Children.Add(Wings);
        }

        public override void OnClick()
        {
            base.OnClick();
            HandleClick();
        }
        protected abstract void HandleClick();


        /// <summary>
        /// Loads the models for all body parts based on a specified path prefix, part prefixes, and a file suffix.
        /// Example: ("Npc/", "FemaleHead", "FemaleUpper", ..., 2) -> "Data/Npc/FemaleHead02.bmd"
        /// </summary>
        protected async Task SetBodyPartsAsync(
            string pathPrefix, string helmPrefix, string armorPrefix, string pantPrefix,
            string glovePrefix, string bootPrefix, int skinIndex)
        {
            // Format skin index to two digits (e.g., 1 -> "01", 10 -> "10")
            string fileSuffix = skinIndex.ToString("D2");

            var tasks = new List<Task>
            {
                LoadPartAsync(Helm, $"{pathPrefix}{helmPrefix}{fileSuffix}.bmd"),
                LoadPartAsync(Armor, $"{pathPrefix}{armorPrefix}{fileSuffix}.bmd"),
                LoadPartAsync(Pants, $"{pathPrefix}{pantPrefix}{fileSuffix}.bmd"),
                LoadPartAsync(Gloves, $"{pathPrefix}{glovePrefix}{fileSuffix}.bmd"),
                LoadPartAsync(Boots, $"{pathPrefix}{bootPrefix}{fileSuffix}.bmd")
            };

            await Task.WhenAll(tasks);
        }

        private async Task LoadPartAsync(ModelObject part, string modelPath)
        {
            if (part != null)
            {
                part.Model = await BMDLoader.Instance.Prepare(modelPath);
                if (part.Model == null)
                {
                    _logger?.LogDebug("Model part not found (this is often normal for NPCs): {Path}", modelPath);
                }
            }
        }

        protected override void UpdateWorldBoundingBox()
        {
            base.UpdateWorldBoundingBox();

            var allCorners = new List<Vector3>(BoundingBoxWorld.GetCorners());

            foreach (var child in Children)
            {
                if (child is ModelObject modelChild && modelChild.Visible && modelChild.Model != null)
                {
                    allCorners.AddRange(modelChild.BoundingBoxWorld.GetCorners());
                }
            }

            if (allCorners.Count > 0)
            {
                BoundingBoxWorld = BoundingBox.CreateFromPoints(allCorners);
            }
        }

        public override void Update(GameTime gameTime)
        {
            bool wasMoving = IsMoving;
            base.Update(gameTime);

            if (Status == GameControlStatus.Ready)
            {
                UpdateWorldBoundingBox();
            }

            if (wasMoving && !IsMoving && !IsOneShotPlaying)
            {
                if (CurrentAction == (int)PlayerAction.PlayerWalkMale || CurrentAction == (int)PlayerAction.PlayerWalkFemale)
                {
                    PlayAction((ushort)PlayerAction.PlayerStopMale);
                }
            }
        }
    }
}
