using PPM.Model;
using PPM1.Model;
using PPM1.Model.PPM1Interface;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace PPM1.Domain
{
    public class RoleManager : RoleInterface
    {
        private static readonly List<Role> _roleList = new List<Role>();

        public ActionResult Add(Role role)
        {
            ActionResult result = new ActionResult() { IsSuccess = true };
            try
            {
                if (_roleList.Count > 0)
                {
                    if (_roleList.Exists(r => r.RoleId == role.RoleId) || _roleList.Exists(r => r.RoleName == role.RoleName))
                    {
                        result.IsSuccess = false;
                        result.Status = "Validation Failed";
                    }
                    else
                    {
                        _roleList.Add(role);
                        result.Status = "Role Added";
                    }
                }
                else
                {
                    _roleList.Add(role);
                    result.Status = "New Role Added";
                }
            }
            catch (Exception e)
            {
                result.IsSuccess = false;
                result.Status = "Exception Occured : " + e.ToString();
            }
            return result;

        }

        public DataResult<Role> ViewListAll()
        {
            DataResult<Role> roleResult = new DataResult<Role>() { IsSuccess = true };
            if (_roleList.Count > 0)
            {
                roleResult.Results = _roleList;
            }
            else
            {
                roleResult.IsSuccess = false;
                roleResult.Status = "No Role in list";
            }
            return roleResult;
        }

        public Role ViewListById(uint id)
        {
            Role role = new Role();
            role = _roleList.Single(r => r.RoleId == id);
            return role;
        }

        public ActionResult ValidRole(uint id)
        {
            ActionResult actionResult = new ActionResult() { IsSuccess = true };
            if (_roleList.Count > 0)
            {
                if (_roleList.Exists(r => r.RoleId == id))
                {
                    actionResult.IsSuccess = true;
                    actionResult.Status = $"Role Id: {id} is validated Successfully!";
                }
                else
                {
                    actionResult.IsSuccess = false;
                    actionResult.Status = $"Role Id : {id} is not present in the Role List!";
                }
            }
            else
            {
                actionResult.IsSuccess = false;
                actionResult.Status = "Role List is Empty!";
            }
            return actionResult;
        }

        public ActionResult ValidRoleByName(string role)
        {
            ActionResult action = new ActionResult() { IsSuccess = true };
            try
            {
                if (_roleList.Exists(r => r.RoleName == role))
                {
                    action.IsSuccess = true;
                }
                else
                {
                    action.IsSuccess = false;
                    action.Status = "Role is not in the Role List" + role;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("error occured" + e.ToString());
                action.IsSuccess = false;
            }
            return action;
        }

        public ActionResult Delete(uint id)
        {
            ActionResult actionResult = new ActionResult() { IsSuccess = true };
            try
            {
                var deleteRole = _roleList.Single(r => r.RoleId == id);
                _roleList.Remove(deleteRole);
                actionResult.Status = "Role Deleted Successfully!";
            }
            catch (Exception)
            {
                actionResult.Status = "Error Occoured at Delete Role!";
                actionResult.IsSuccess = false;
            }
            return actionResult;
        }

        public ActionResult ToXmlSerialization()
        {
            ActionResult actionResult = new ActionResult() { IsSuccess = true };
            try
            {
                if (_roleList.Count > 0)
                {
                    /*string filePath = System.IO.Path.Combine(System.Web.HttpContext.Current.Server.MapPath("F:\\PPM\\PPM.Model"), "AppData", fileName)
                    var filePath = System.IO.File.Create(Environment.GetFolderPath(Environment.SpecialFolder.MyMusic) + fileName)
                    XmlSerializer serializer = new XmlSerializer(typeof(List<Role>));
                    using (TextWriter tw = new StreamWriter(fileName))
                    {
                        serializer.Serialize(tw, _roleList);
                        tw.Close();
                    }*/
                    XmlSerializer RolexmlSerializer = new XmlSerializer(typeof(List<Role>));
                    TextWriter RoleFilestream = new StreamWriter(@"C:\Users\hp\source\repos\roles.xml");
                    RolexmlSerializer.Serialize(RoleFilestream, o: RoleManager._roleList);
                    RoleFilestream.Close();

                }
                else
                {
                    actionResult.IsSuccess = false;
                    actionResult.Status = "Role list is Empty";
                }

            }
            catch (Exception e)
            {
                actionResult.IsSuccess = false;
                actionResult.Status = "Error Occoured!" + e.Message;
            }
            return actionResult;
        }

        public ActionResult ToTxtFile(string fileName)
        {
            ActionResult actionResult = new ActionResult() { IsSuccess = true };
            try
            {
                if (_roleList.Count > 0)
                {
                    using (TextWriter sw = new StreamWriter(fileName))
                    {
                        foreach (Role r in _roleList)
                        {
                            sw.WriteLine("Role Id: " + r.RoleId + "\nRole Name: " + r.RoleName);
                            sw.WriteLine("--------------------------------------------------------------------------------------");
                            actionResult.Status = "Role Is Saved in The Text File!";
                        }
                    }
                }
                else
                {
                    actionResult.IsSuccess = false;
                    actionResult.Status = "Role list is empty!";
                }
            }
            catch (Exception e)
            {
                actionResult.IsSuccess = false;
                actionResult.Status = "Error Occoured" + "\n" + e.Message;
            }

            return actionResult;
        }

        public ActionResult ToAdoDB()
        {
            ActionResult actionResult = new ActionResult() { IsSuccess = true };
            string conn = "Server=DESKTOP-NNAFOET\\SQLEXPRESS; Database=PPM;Integrated security=true;TrustServerCertificate=true";
            SqlConnection myConn = new SqlConnection(conn);
            string str = "DROP TABLE IF EXISTS role";
            try
            {
                myConn.Open();
                using (SqlCommand command = new SqlCommand(str, myConn))
                {
                    command.ExecuteNonQuery();
                    actionResult.Status = "Old Data of Role Dropped Successfully!";
                }
            }
            catch (Exception e)
            {
                actionResult.IsSuccess = false;
                actionResult.Status = e.Message;
            }
            finally
            {
                if (myConn.State == ConnectionState.Open)
                {
                    myConn.Close();
                }
            }
            if (actionResult.IsSuccess)
            {
                try
                {
                    myConn.Open();
                    using (SqlCommand command = new SqlCommand("CREATE TABLE role (RoleId int,RoleName varchar(50));", myConn))
                    {
                        command.ExecuteNonQuery();

                        foreach (Role role in _roleList)
                        {
                            int id = (int)role.RoleId;
                            string insertQ = "INSERT INTO role values(@RoleId,@RoleName)";
                            SqlCommand command1 = new SqlCommand(insertQ, myConn);
                            command1.Parameters.AddWithValue("@RoleId", id);
                            command1.Parameters.AddWithValue("@RoleName", role.RoleName);
                            command1.ExecuteNonQuery();
                        }
                        actionResult.Status = actionResult.Status + "\n" + "Table role Added SuccessFully!";
                    }

                }
                catch (Exception e)
                {
                    actionResult.IsSuccess = false;
                    actionResult.Status = e.Message;
                }
                finally
                {
                    if (myConn.State == ConnectionState.Open)
                    {
                        myConn.Close();
                    }
                }
            }
            return actionResult;
        }

        public ActionResult ToEFDB()
        {
            ActionResult actionResult = new ActionResult() { IsSuccess = true };
            try
            {
                if (_roleList.Count > 0)
                {
                    using (var db = new ProgramDbContext())
                    {
                        List<Role> roleList = db.Roles.ToList();
                        foreach (Role role in _roleList)
                        {
                            if (roleList.Exists(r => r.RoleId == role.RoleId))
                            {
                                var r = roleList.Single(r => r.RoleId == role.RoleId);
                                db.Roles.Remove(r);
                                db.SaveChanges();

                                db.Add(role);
                                db.SaveChanges();
                            }
                            else
                            {
                                db.Roles.Add(role);
                                db.SaveChanges();
                            }

                        }
                        actionResult.Status = "Role Added to Database Successfully!";
                    }
                }
                else
                {
                    actionResult.IsSuccess = false;
                    actionResult.Status = "Role List is Empty!";
                }
            }
            catch (Exception e)
            {
                actionResult.IsSuccess = false;
                actionResult.Status = e.Message;
            }
            return actionResult;
        }

    }
}
