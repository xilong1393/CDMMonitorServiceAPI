using MonitorAPI.Service.Operations;

namespace MonitorAPI.Models.OperationModel
{
    public class PPCIPCReturnParameter
    {
        public CommandParameter PPCReturnParameter { get; set; }
        public CommandParameter IPCReturnParameter { get; set; }
        public PPCIPCReturnParameter(CommandParameter ppc, CommandParameter ipc)
        {
            PPCReturnParameter = ppc;
            IPCReturnParameter = ipc;
        }
    }
}