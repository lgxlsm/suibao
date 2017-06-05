using SuiBao_v1.Model;

namespace SuiBao_v1.Business
{
    public class BaseBusiness
    {
        [Ninject.Inject]
        protected IRepository.IRepositoryFactory _repositoryFactory { get; set; }

        /// <summary>
        /// 数据库上下文对象
        /// </summary>
        protected wolf_testEntities dbContext => OperationContext.Current;


        /// <summary>
        /// 业务层统一的返回信息格式
        /// </summary>
        /// <param name="status"></param>
        /// <param name="msg"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        protected ReturnMessage BllResponse(ResultStatus status, string msg = "", object data = null, string url = "")
        {
            return new ReturnMessage(status, msg, data, url);
        }
    }
}
