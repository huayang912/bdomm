using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Configuration;
using App.Core.Extensions;


namespace App.Data
{
    public class UtilityDAO
    {
        private static String ConnectionString
        {
            get
            {
                String connectionString = ConfigurationManager.ConnectionStrings["OMMConnectionString"].ConnectionString;
                ///Following contains the Fix for Entity Framework
                //connectionString = connectionString.Substring(connectionString.IndexOf("\"")).Replace("\"", String.Empty);
                return connectionString;
            }
        }
        public UtilityDAO()
        {
        }

        public DataSet GetDataSet(String SQL, DbParameter[] parameters, bool isStoredProcedure)
        {
            SqlConnection conn = new SqlConnection(ConnectionString);
            DataSet ds = new DataSet("CustomeQueryResult");
            try
            {
                if (conn.State != ConnectionState.Open)
                    conn.Open();

                SqlCommand command = new SqlCommand(SQL, conn);
                command.CommandType = isStoredProcedure ? CommandType.StoredProcedure : CommandType.Text;
                if (parameters != null && parameters.Count() > 0)
                {
                    foreach (DbParameter parameter in parameters)
                    {                        
                        command.Parameters.Add(new System.Data.SqlClient.SqlParameter(parameter.Name, parameter.Value));//command.Parameters.Add(parameter);
                    }
                }
                SqlDataAdapter da = new SqlDataAdapter(command);
                da.Fill(ds);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                    conn.Close();
            }
            return ds;
        }
        /// <summary>
        /// Gets Paged Data by Executing an SQL
        /// </summary>
        /// <param name="SQL"></param>
        /// <param name="parameters"></param>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalRecord"></param>
        /// <returns></returns>
        public DataSet GetPagedData(String SQL, DbParameter[] parameters, int pageNumber, int pageSize, out int totalRecord)
        {
            totalRecord = 0;
            int startRecord = (pageSize * (pageNumber - 1)) + 1;
            int endRecord = pageSize * pageNumber;
            SQL = string.Format(SQL, startRecord, endRecord);

            SqlConnection conn = new SqlConnection(ConnectionString);
            DataSet ds = new DataSet("CustomeQueryResult");
            try
            {
                if (conn.State != ConnectionState.Open)
                    conn.Open();

                SqlCommand command = new SqlCommand(SQL, conn);
                command.CommandType = CommandType.Text;
                if (parameters != null && parameters.Count() > 0)
                {
                    foreach (DbParameter parameter in parameters)
                    {
                        command.Parameters.Add(new System.Data.SqlClient.SqlParameter(parameter.Name, parameter.Value));
                    }
                }
                SqlDataAdapter da = new SqlDataAdapter(command);
                da.Fill(ds);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                    conn.Close();
            }
            if (!ds.IsEmpty())
            {
                if (ds.Tables[0].Rows.Count > 0)
                    totalRecord = NullHandler.GetInt(ds.Tables[0].Rows[0]["TotalRecord"]);
            }
            return ds;
        }

        /// <summary>
        /// Gets Scalar Value Object by Executing an SQL
        /// </summary>
        /// <param name="SQL"></param>
        /// <param name="parameters"></param>
        /// <param name="isStoredProcedure"></param>
        /// <returns></returns>
        public object GetScalar(String SQL, DbParameter[] parameters, bool isStoredProcedure)
        {
            SqlConnection conn = new SqlConnection(ConnectionString);
            try
            {
                if (conn.State != ConnectionState.Open)
                    conn.Open();

                SqlCommand command = new SqlCommand(SQL, conn);
                command.CommandType = isStoredProcedure ? CommandType.StoredProcedure : CommandType.Text;
                if (parameters != null && parameters.Count() > 0)
                {
                    foreach (DbParameter parameter in parameters)
                        command.Parameters.Add(new System.Data.SqlClient.SqlParameter(parameter.Name, parameter.Value));//command.Parameters.Add(parameter);
                }
                return command.ExecuteScalar();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                    conn.Close();
            }
            //return null;
        }

        #region Dynamic Search Query Methods
//        public DataSet SerchProject(int customerID, int propertyTypeID, double sizeFrom, double sizeTo, int statusID, int cityID,
//            int zoneID, int brochureFileTypeID, int pageNumber, int pageSize, out int totalRecord)
//        {
//            totalRecord = 0;

//            StringBuilder SQL = new StringBuilder(10);
//            SQL.AppendFormat(@"
//                WITH SearchedProjects AS 
//                (
//                    SELECT P.ID
//	                    , P.Name AS [ProjectName] 
//	                    , P.PricePerUnitFrom
//	                    , P.PricePerUnitTo
//	                    , P.Description
//                        , P.ShowPricingInfo
//	                    , PS.Name AS [ProjectStatus]
//	                    , C.Name As [Company]
//	                    , Z.Name AS [Zone]
//	                    , City.Name AS [City]
//	                    , Country.Name AS [Country]
//	                    , U.Name AS [UnitName]                        
//	                    , ROW_NUMBER() OVER(ORDER BY P.IsHot DESC, P.Name) AS [RowNumber]
//                    FROM Project P
//                        INNER JOIN Customer C ON C.ID = P.CustomerID
//                        INNER JOIN PropertyType PT ON PT.ID = P.PropertyTypeID
//                        INNER JOIN ProjectStatus PS ON PS.ID = P.ProjectStatusID
//                        INNER JOIN Unit U ON U.ID = P.UnitID
//                        INNER JOIN Zone Z ON Z.ID = P.ZoneID
//                        INNER JOIN City ON City.ID = Z.CityID
//                        INNER JOIN Country ON Country.ID = City.CountryID
//                        INNER JOIN PackageSubscription PSUB ON PSUB.ID = P.PackageSubscriptionID
//                    WHERE C.IsApproved = 1
//                        AND P.IsApproved = 1
//                        AND C.IsDeleted = 0
//                        AND P.IsDeleted = 0 
//                        AND PSUB.ExpireDate >= GETDATE()");

//            if (customerID > 0)
//                SQL.AppendFormat(" AND C.ID = {0} ", customerID);
//            if (propertyTypeID > 0)
//                SQL.AppendFormat(" AND P.PropertyTypeID = {0} ", propertyTypeID);
//            if (sizeFrom > 0)
//                SQL.AppendFormat(" AND P.Area >= {0} ", sizeFrom);
//            if (sizeTo > 0)
//                SQL.AppendFormat(" AND P.Area <= {0} ", sizeTo);
//            if (statusID > 0)
//                SQL.AppendFormat(" AND PS.ID = {0} ", statusID);
//            if (cityID > 0)
//                SQL.AppendFormat(@" AND P.ZoneID IN
//                                (
//	                                SELECT ID FROM Zone WHERE Zone.CityID = {0}
//                                )", cityID);
//            if (zoneID > 0)
//                SQL.AppendFormat(" AND P.ZoneID = {0}", zoneID);

//            SQL.AppendFormat(@"
//                )
//                SELECT * 
//                    , (SELECT TOP 1 ThumbFileName FROM ProjectFile WHERE ProjectID = SearchedProjects.ID AND FileTypeID != {0}) AS [ProjectImage] ", brochureFileTypeID);
//            SQL.Append(@"
//	                , (SELECT MAX(RowNumber) FROM SearchedProjects) AS TotalRecord
//                FROM SearchedProjects
//                WHERE [RowNumber] BETWEEN {0} AND {1}");

//            return GetPagedData(SQL.ToString(), null, pageNumber, pageSize, out totalRecord);
//        }

//        public DataSet SerchProjectForAdmin(int customerID, int propertyTypeID, bool? isApproved, int pageNumber, int pageSize, out int totalRecord)
//        {
//            totalRecord = 0;

//            StringBuilder SQL = new StringBuilder(10);
//            SQL.AppendFormat(@"
//                WITH Projects AS
//                (
//	                SELECT Project.ID 
//		                , Project.Name
//		                , Project.Description
//		                , Project.ShowPricingInfo
//		                , ProjectStatus.Name AS [Status]
//                        , PropertyType.Name AS [PropertyType]
//		                , City.Name AS City 
//		                , Zone.Name AS Zone 
//                        , IsHot As [Is Featured]
//		                , ROW_NUMBER() OVER (ORDER BY Project.Name) AS RowNumber
//	                FROM Project
//                        INNER JOIN PropertyType ON PropertyType.ID = Project.PropertyTypeID
//                        INNER JOIN Customer C ON C.ID = Project.CustomerID
//		                INNER JOIN ProjectStatus ON ProjectStatus.ID = Project.ProjectStatusID
//		                INNER JOIN Zone ON Zone.ID = Project.ZoneID
//		                INNER JOIN City ON City.ID = Zone.CityID
//	                WHERE Project.IsDeleted = 0 
//                        AND C.IsApproved = 1 
//                        AND C.IsDeleted = 0 ");

//            if (customerID > 0)
//                SQL.AppendFormat(" AND C.ID = {0} ", customerID);
//            if (propertyTypeID > 0)
//                SQL.AppendFormat(" AND PropertyType.ID = {0} ", propertyTypeID);
//            if (isApproved != null)
//                SQL.AppendFormat(" AND Project.IsApproved = {0} ", isApproved.GetValueOrDefault() == true ? 1 : 0);

//            SQL.Append(@"
//                )
//                SELECT *
//	                , (SELECT COUNT(*) FROM Projects) AS TotalRecord
//                FROM Projects
//                WHERE RowNumber BETWEEN {0} AND {1}");

//            return GetPagedData(SQL.ToString(), null, pageNumber, pageSize, out totalRecord);
//        }

//        public DataSet SearchCompanies(int businessTypeID, String startsWith, int pageNumber, int pageSize, out int totalRecord)
//        {
//            totalRecord = 0;
//            StringBuilder SQL = new StringBuilder(10);
//            SQL.Append(@"
//                    WITH Companies AS
//                    (
//	                    SELECT C.ID
//		                    , C.Name AS [CompanyName]
//                            , C.WebSite
//		                    , C.About
//		                    , C.Logo
//		                    , City.Name AS [City]
//		                    , Country.Name AS [Country]
//		                    , ROW_NUMBER() OVER (ORDER BY C.Name) AS [RowNumber]
//	                    FROM Customer C
//		                    LEFT JOIN Address ON Address.CustomerID = C.ID
//		                    LEFT JOIN City ON City.ID = Address.CityID
//		                    LEFT JOIN Country ON Country.ID = City.CountryID
//	                    WHERE BusinessTypeID IN
//	                    (
//                            SELECT ID FROM BusinessType 
//		                    WHERE (ID = @BusinessTypeID OR ParentID = @BusinessTypeID)		                    
//	                    )
//	                    AND C.IsIndividual = 0
//	                    AND C.IsDeleted = 0
//	                    AND C.IsApproved = 1 ");
//            if (!startsWith.IsNullOrEmpty())
//                SQL.AppendFormat(@"
//                        AND C.Name LIKE '{0}%'", startsWith.ToSqlSafeData());

//            SQL.Append(@"
//                    )
//                    SELECT * 
//	                    , (SELECT COUNT(*) FROM Project WHERE CustomerID = Companies.ID AND Project.IsDeleted = 0) AS [ProjectCount]
//	                    , (SELECT MAX(RowNumber) FROM Companies) AS TotalRecord
//                    FROM Companies
//                    WHERE [RowNumber] BETWEEN {0} AND {1}");
//            //SELECT Child.ID FROM BusinessType Parent
//            //        INNER JOIN BusinessType Child ON Child.ParentID = Parent.ID
//            //WHERE (Child.ID = @BusinessTypeID OR Child.ParentID = @BusinessTypeID)
//            DbParameter[] parameters = new[] { new DbParameter("@BusinessTypeID", businessTypeID) };
//            return GetPagedData(SQL.ToString(), parameters, pageNumber, pageSize, out totalRecord);
//        }
//        public DataSet SearchMemberCompanies(int packageID, String keyword, String startsWith, int pageNumber, int pageSize, out int totalRecord)
//        {
//            StringBuilder SQL = new StringBuilder(@"
//                    WITH Companies AS
//                    (
//                        SELECT C.ID
//                            , C.Name AS [CompanyName]
//                            , C.WebSite
//                            , C.About
//                            , C.Logo
//                            , City.Name AS [City]
//                            , Country.Name AS [Country]
//                            , Package.Name AS [Package Name]
//                            , ROW_NUMBER() OVER (ORDER BY Package.ID, C.Name) AS [RowNumber]
//                        FROM Customer C
//                            LEFT JOIN Address ON Address.CustomerID = C.ID
//                            LEFT JOIN City ON City.ID = Address.CityID
//                            LEFT JOIN Country ON Country.ID = City.CountryID
//                            LEFT JOIN PackageSubscription PS ON PS.CustomerID = C.ID
//                            LEFT JOIN Package ON Package.ID = PS.PackageID
//                        WHERE BusinessTypeID IN
//                        (
//                            SELECT ID FROM BusinessType 
//                            WHERE (ID = 1 OR ParentID = 1)		                    
//                        )    
//                        AND C.IsIndividual = 0
//                        AND C.IsDeleted = 0
//                        AND C.IsApproved = 1
//                        AND Package.Name IS NOT NULL ");
//            if (!startsWith.IsNullOrEmpty())
//                SQL.AppendFormat(" AND C.Name LIKE '{0}%'", startsWith);
//            if (packageID > 0)
//                SQL.AppendFormat(" AND Package.ID = @PackageID ");
//            if (!keyword.IsNullOrEmpty())
//                SQL.AppendFormat(" AND C.Name LIKE '%{0}%'", keyword.ToSqlSafeData());

//            SQL.Append(@"
//                    )
//                    SELECT * 
//                        , (SELECT COUNT(*) FROM Project WHERE CustomerID = Companies.ID AND Project.IsDeleted = 0) AS [ProjectCount]
//                        , (SELECT MAX(RowNumber) FROM Companies) AS TotalRecord
//                    FROM Companies
//                    WHERE [RowNumber] BETWEEN {0} AND {1} ");
//            List<DbParameter> parameters = new List<DbParameter>();
//            if (packageID > 0)
//                parameters.Add(new DbParameter("@PackageID", packageID));

//            return GetPagedData(SQL.ToString(), parameters.Count > 0 ? parameters.ToArray() : null, pageNumber, pageSize, out totalRecord);
//        }
//        public int GetActiveCountOfServiceProviders(int businessTypeID)
//        {
//            String SQL = @"SELECT COUNT(*) FROM Customer WHERE BusinessTypeID IN
//                            (
//                                SELECT C.ID FROM BusinessType P
//		                             INNER JOIN BusinessType C ON C.ParentID = P.ID
//	                            WHERE (C.ID = @BusinessTypeID OR C.ParentID = @BusinessTypeID)	                            
//                            )
//                        AND Customer.IsIndividual = 0
//                        AND Customer.IsDeleted = 0
//                        AND Customer.IsApproved = 1";
//            //SELECT ID FROM BusinessType WHERE (ID = @BusinessTypeID OR ParentID = @BusinessTypeID)
//            DbParameter[] parameters = new[] { new DbParameter("@BusinessTypeID", businessTypeID) };
//            object count = GetScalar(SQL, parameters, false);
//            return NullHandler.GetInt(count);
//        }
//        /// <summary>
//        /// Gets Paged Proeperties for rent by Country, City, Zone and Rent Type
//        /// </summary>
//        /// <param name="countryID"></param>
//        /// <param name="cityID"></param>
//        /// <param name="zoneID"></param>
//        /// <param name="rentTypeID"></param>
//        /// <param name="pageNumber"></param>
//        /// <param name="pageSize"></param>
//        /// <param name="totalRecord"></param>
//        /// <returns></returns>
//        public DataSet SearchPropertyForRent(int cityID, int zoneID, int rentTypeID, int pageNumber, int pageSize, out int totalRecord)
//        {
//            totalRecord = 0;
//            StringBuilder SQL = new StringBuilder();
//            SQL.Append(@"
//                    WITH PropertyForRents AS
//                    (
//                        SELECT PTR.ID
//                            , Address.Phone
//		                    , Address.Fax
//		                    , Address.Email                                
//                            , C.Name AS [CompanyName]
//                            , PTR.Size
//                            , PTR.NoOfBeds
//                            , PTR.Address     
//                            , PTR.MonthlyRent
//                            , City.Name AS [City]		                        
//                            , Zone.Name AS [Zone]
//                            , PTR.ServiceCharge		                        
//                            , PTR.ExpireDate
//                            , PTR.ThumbImage
//                            , PTR.LargeImage
//                            , PTR.Remark
//                            , RT.Name AS [RentType]
//                            , RS.Name AS [RentStatus]		                        
//                            , Country.Name AS [Country] 
//                            , ROW_NUMBER() OVER (ORDER BY PTR.RentStatusID, PTR.Created DESC) AS RowNumber
//                        FROM PropertyToRent PTR
//                            INNER JOIN Customer C ON C.ID = PTR.CompanyID
//                            INNER JOIN RentType RT ON RT.ID = PTR.RentTypeID
//                            INNER JOIN RentStatus RS ON RS.ID = PTR.RentStatusID
//                            INNER JOIN Zone ON Zone.ID = PTR.ZoneID
//                            INNER JOIN City ON City.ID = Zone.CityID
//                            INNER JOIN Country ON Country.ID = City.CountryID
//                            LEFT JOIN Address ON Address.CustomerID = C.ID		
//                        WHERE PTR.IsDeleted = 0
//		                    AND PTR.IsActive = 1
//                            -- AND PTR.RentStatusID = 1
//		                    AND PTR.ExpireDate >= GETDATE() ");
//            //if (countryID > 0)
//            //    SQL.AppendFormat(" AND Country.ID = {0} ", countryID);
//            if (cityID > 0)
//                SQL.AppendFormat(" AND City.ID = {0} ", cityID);
//            if (zoneID > 0)
//                SQL.AppendFormat(" AND PTR.ZoneID = {0} ", zoneID);
//            if (rentTypeID > 0)
//                SQL.AppendFormat(" AND PTR.RentTypeID = {0} ", rentTypeID);

//            SQL.Append(@"
//                    )
//                    SELECT *
//                        , (SELECT MAX(RowNumber) FROM PropertyForRents) AS TotalRecord  
//                    FROM PropertyForRents
//                    WHERE RowNumber BETWEEN {0} AND {1}");
//            return GetPagedData(SQL.ToString(), null, pageNumber, pageSize, out totalRecord);
//        }
        #endregion Dynamic Search Query Methods
    }
}
