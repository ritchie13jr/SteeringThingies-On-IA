
using BTs;

public class ACTION_MakeWithdrawal : Action
{
    
    public override Status OnTick ()
    {
        if (((BOB_Blackboard)blackboard).MakeWithdrawal())
            return Status.SUCCEEDED;
        else
            return Status.FAILED;
    }

}
