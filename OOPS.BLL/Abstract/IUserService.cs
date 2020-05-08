﻿using OOPS.Core.Business;
using OOPS.DTO.ProjectBase;
using System;
using System.Collections.Generic;
using System.Text;

namespace OOPS.BLL.Abstract
{
    public interface IUserService : IServiceBase
    {
        UserDTO LoginUser(UserDTO loginUser);
        List<UserDTO> getAll();
        List<UserDTO> getAllUserinRole(int RoleID);
        UserDTO newUser(UserDTO user);
        UserDTO updateUser(UserDTO user);
        UserDTO updateUserRole(UserDTO user);
        UserDTO updateUserCompany(UserDTO user);
        bool deleteUser(int userId);
        UserDTO getUser(int Id);
        UserDTO FindwithUserName(string userName);
        UserDTO FindwithMail(string mail);
        UserDTO FindwithUsernameandMail(string mailorUserName, string password);
    }
}
