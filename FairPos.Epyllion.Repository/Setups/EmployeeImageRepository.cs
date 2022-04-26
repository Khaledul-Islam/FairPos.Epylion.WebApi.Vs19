using FairPos.Epylion.Models.Setups;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FairPos.Epyllion.Repository.Setups
{
    ////////////////////////////////////////////////////////////////////////////////////////////////////////
    //FileName: MeasureUnitRepository.cs
    //FileType: C# Source file
    //Author : SHUVO
    //Created On : 29/09/2021 
    //Last Modified On :
    //Copy Rights : MediaSoft Data System LTD
    //Description : Interface and Class for defining database related functions
    ////////////////////////////////////////////////////////////////////////////////////////////////////////
    public interface IEmployeeImageRepository
    {
        EmployeeImage FindById(string id);
    }

    public class EmployeeImageRepository : BaseRepository, IEmployeeImageRepository
    {
        public EmployeeImageRepository(IDBConnectionProvider dBConnectionProvider) : base(dBConnectionProvider)
        {
            baseQuery = @"SELECT [EmpID]
                        ,[EmpImage]
                        ,CASE WHEN (ROW_NUMBER() OVER (ORDER BY EmpID)  =1)
	                     THEN	(SELECT COUNT(*) FROM EmployeeImage) 
	                     ELSE 0 END RecordCount
                    from EmployeeImage";
        }
        public EmployeeImage FindById(string id)
        {
            query = baseQuery + $" where EmpID='{id}' ";
            var response = _dal.SelectFirstOrDefault<EmployeeImage>(query, ref msg);

            if (!string.IsNullOrEmpty(msg))
                throw new Exception(msg);

            return response;
        }
    }
}
