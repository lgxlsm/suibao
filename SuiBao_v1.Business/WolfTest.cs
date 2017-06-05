using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SuiBao_v1.Model;

namespace SuiBao_v1.Business
{
    public class WolfTest : BaseBusiness, IBusiness.IWolfTest
    {
        public ReturnMessage Test1()
        {
            var list = _repositoryFactory.People.Where(x => x.Num != 0);
            return BllResponse(ResultStatus.Success, ResponseTip.ActionSuccess);
        }

        public ReturnMessage Test2()
        {
            People people = new People() { Name = "123123", CreateTime = DateTime.Now, SchoolNum = 1 };
            School school = new School() { Name = "123123", Remark = "66666666666666" };
            _repositoryFactory.School.Add(school);
            _repositoryFactory.People.Add(people);
            int result = _repositoryFactory.SaveChanges();
            if (result > 0)
            {
                return BllResponse(ResultStatus.Success, ResponseTip.ActionSuccess, result);
            }
            else
            {
                return BllResponse(ResultStatus.ServiceError, ResponseTip.ServiceError);
            }
        }
    }
}
