using Vintagestory.API.Common;
using Vintagestory.API.MathTools;
using Vintagestory.GameContent;

namespace betterhewnfencegates;

public sealed class BetterHewnFenceGates : ModSystem {
    public override void Start(ICoreAPI api) {
        api.RegisterBlockClass("BlockBetterHewnFenceGate", typeof(BlockBetterHewnFenceGate));
    }

    private class BlockBetterHewnFenceGate : BlockFenceGateRoughHewn {
        public override bool TryPlaceBlock(IWorldAccessor world, IPlayer byPlayer, ItemStack itemstack, BlockSelection blockSel, ref string failureCode) {
            if (!CanPlaceBlock(world, byPlayer, blockSel, ref failureCode)) {
                return false;
            }

            string facing = SuggestedHVOrientation(byPlayer, blockSel)[0].Code[..1];
            AssetLocation code = CodeWithVariants(new[] { "type", "state" }, new[] { facing, "closed" });
            world.BlockAccessor.SetBlock(world.BlockAccessor.GetBlock(code).BlockId, blockSel.Position);
            return true;
        }

        public override AssetLocation GetRotatedBlockCode(int angle) {
            BlockFacing blockFacing1 = BlockFacing.FromFirstLetter(Variant["type"]);
            BlockFacing blockFacing2 = BlockFacing.HORIZONTALS_ANGLEORDER[(blockFacing1.HorizontalAngleIndex + angle / 90) % 4];
            string str = Variant["type"];
            if (blockFacing1.Axis != blockFacing2.Axis) {
                str = str switch {
                    "n" => "w",
                    "s" => "e",
                    "w" => "s",
                    _ => "n"
                };
            }

            return CodeWithVariant("type", str);
        }
    }
}
