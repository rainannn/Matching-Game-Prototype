public class JumpCommand : ICommand
{
    private readonly ItemController itemController;
    private readonly Slot slot;

    public JumpCommand(ItemController itemController, Slot slot)
    {
        this.itemController = itemController;
        this.slot = slot;
    }

    public void Execute()
    {
        itemController.ShiftRight(slot);
    }
}