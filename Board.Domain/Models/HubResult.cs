namespace Board.Common.Models
{
    public class HubResult
    {
        public HubResultCode Code { get; set; }
        public object Data { get; set; }

        public HubResult(HubResultCode code = HubResultCode.Undefined, object data = null)
        {
            Code = code;
            Data = data;
        }

        public static HubResult Ok(object data = null)
        {
            return new HubResult(HubResultCode.OK, data);
        }

        public static HubResult Fail(object error = null)
        {
            return new HubResult(HubResultCode.ERROR, error);
        }

        public static HubResult Fail(HubResultCode code, object error = null)
        {
            return new HubResult(code, error);
        }
    }

    public enum HubResultCode
    {
        Undefined = 0,
        //200-299 OK
        OK = 200,
        ERROR = 400
    }
}
