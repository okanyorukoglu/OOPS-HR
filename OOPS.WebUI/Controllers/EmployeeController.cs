﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using OOPS.BLL.Abstract;
using OOPS.BLL.Abstract.CompanyAbstract;
using OOPS.BLL.Abstract.EmployeeAbstract;
using OOPS.BLL.Abstract.StaticAbstract;
using OOPS.DTO.Employee;
using OOPS.DTO.ProjectBase;
using OOPS.WebUI.Models;

namespace OOPS.WebUI.Controllers
{
    public class EmployeeController : BaseController
    {
        private IEmployeeService service;
        private IEmployeeDetailService employeeDetailService;
        private IEmployeeOtherInfoService employeeOtherInfoService;
        private IBankAccountTypeService bankAccountTypeService;
        private IAccessTypeService accessTypeService;
        private IBloodGroupService bloodGroupService;
        private ICityService cityService;
        private IContractTypeService contractType;
        private ICountryService countryService;
        private IDisabilitySituationService disabilitySituationService;
        private IEducationLevelService educationLevelService;
        private IEducationStatusService educationStatusService;
        private IEmploymentTypeService employmentTypeService;
        private IGenderService genderService;
        private IMaritalStatusService maritalStatusService;
        private IPositionService positionService;
        public EmployeeController(IEmployeeService _service, IEmployeeDetailService _employeeDetailService, IEmployeeOtherInfoService _employeeOtherInfoService,
            IBankAccountTypeService _bankAccountTypeService, IAccessTypeService _accessTypeService, IBloodGroupService _bloodGroupService, ICityService _cityService,
            IContractTypeService _contractType, ICountryService _countryService, IDisabilitySituationService _disabilitySituationService, IEducationLevelService _educationLevelService,
            IEducationStatusService _educationStatusService, IEmploymentTypeService _employmentTypeService, IGenderService _genderService,
            IMaritalStatusService _maritalStatusService, IPositionService _positionService)
        {
            service = _service;
            employeeDetailService = _employeeDetailService;
            employeeOtherInfoService = _employeeOtherInfoService;
            bankAccountTypeService = _bankAccountTypeService;
            accessTypeService = _accessTypeService;
            bloodGroupService = _bloodGroupService;
            cityService = _cityService;
            contractType = _contractType;
            countryService = _countryService;
            disabilitySituationService = _disabilitySituationService;
            educationLevelService = _educationLevelService;
            educationStatusService = _educationStatusService;
            employmentTypeService = _employmentTypeService;
            genderService = _genderService;
            maritalStatusService = _maritalStatusService;
            positionService = _positionService;


        }

        public IActionResult Index()
        {
            var RoleName = CurrentUser.Role.Name;
            int companyId = (int)CurrentUser.CompanyID;

            //Giriş yapan kullanıcının EMployee Id boş degılse = ? Detail sayfasını çapırmak lazım.
            if (RoleName == "Admin")
            {
                return RedirectToAction(nameof(List));
            }
            else
            {
                return RedirectToAction(nameof(DetailEmployee));
            }
        }

        public IActionResult List()
        {
            int companyId = (int)CurrentUser.CompanyID;
            List<EmployeeDTO> employee = service.getCompanyEmployees(companyId);
            //o firmadli çalışanlar lsitelenecek
            return View(employee);
        }

        public IActionResult EditEmployee(int id)
        {
            ViewBag.AccessType = new SelectList(accessTypeService.getAll(), "Id", "AccessTypeName");
            ViewBag.BankAccountType = new SelectList(bankAccountTypeService.getAll(), "Id", "BankAccountTypeName");
            ViewBag.BloodGroup = new SelectList(bloodGroupService.getAll(), "Id", "BloodKind");
            ViewBag.City = new SelectList(cityService.getAll(), "Id", "Name");
            ViewBag.ContractType = new SelectList(contractType.getAll(), "Id", "ContractName");
            ViewBag.Country = new SelectList(countryService.getAll(), "Id", "CountryName");
            ViewBag.Disability = new SelectList(disabilitySituationService.getAll(), "Id", "DisabilityName");
            ViewBag.EducationLevel = new SelectList(educationLevelService.getAll(), "Id", "EducationLevelName");
            ViewBag.EducationStatus = new SelectList(educationStatusService.getAll(), "Id", "StatusName");
            ViewBag.EmploymentType = new SelectList(employmentTypeService.getAll(), "Id", "EmploymentTypeName");
            ViewBag.Gender = new SelectList(genderService.getAll(), "Id", "GenderName");
            ViewBag.MaritalStatus = new SelectList(maritalStatusService.getAll(), "Id", "StatusName");
            EmployeeModel model = new EmployeeModel();
            model.Employee = service.getEmployee(id);
            var empDetail = employeeDetailService.getEmployeeDetail(id);
            var empOtherInfo = employeeOtherInfoService.getEmployeeOtherInfo(id);
            if (empDetail == null)
            {
                model.EmployeeDetail = new EmployeeDetailDTO();
            }
            else
            {
                model.EmployeeDetail = empDetail;
            }
            if (empOtherInfo==null)
            {
                model.EmployeeOtherInfo = new EmployeeOtherInfoDTO();
            }
            else
            {
                model.EmployeeOtherInfo = empOtherInfo;
            }
            //kullanıcının detayı ve update işlemi

            return View(model);
        }

        [HttpPost]
        public IActionResult EditEmployee(EmployeeModel employeeModel)
        {
            if (ModelState.IsValid)
            {
                service.updateEmployee(employeeModel.Employee);

                return RedirectToAction("List");
            }
            else
            {
                var errors = ModelState.Select(x => x.Value.Errors)
                             .Where(y => y.Count > 0)
                             .ToList();
                return View();
            }
        }

        //EmployeeDetail Update and Add
        public IActionResult EditEmployeedetail(EmployeeModel employeeModel)
        {
            if (ModelState.IsValid)
            {
                if (employeeModel.EmployeeDetail.Id == null)
                {
                    //employeeModel.EmployeeDetail.Employee = service.getEmployee(employeeModel.Employee.Id);
                    employeeModel.EmployeeDetail.EmployeeID =employeeModel.Employee.Id;
                    employeeDetailService.newEmployeeDetail(employeeModel.EmployeeDetail);
                    //employeeModel.EmployeeDetail.Employee = service.getEmployee(employeeModel.Employee.Id);
                    //employeeDetailService.updateEmployeeDetail(employeeModel.EmployeeDetail);
                }
                else
                {
                    employeeModel.EmployeeDetail.Employee = service.getEmployee(employeeModel.Employee.Id);
                    employeeDetailService.updateEmployeeDetail(employeeModel.EmployeeDetail);
                }
                
               
                return RedirectToAction("List");
            }
            else
            {
                var errors = ModelState.Select(x => x.Value.Errors)
                             .Where(y => y.Count > 0)
                             .ToList();
                return View();
            }
        }

        //EmployeeOtherInfo Update and Add
        public IActionResult EditEmployeeOtherInfo(EmployeeModel employeeModel)
        {
            if (ModelState.IsValid)
            {
                if (employeeModel.EmployeeOtherInfo.Id == null)
                {          
                    employeeModel.EmployeeOtherInfo.EmployeeID = employeeModel.Employee.Id;
                    employeeOtherInfoService.newEmployeeOtherInfo(employeeModel.EmployeeOtherInfo);
                }
                else
                {
                    employeeModel.EmployeeOtherInfo.Employee = service.getEmployee(employeeModel.Employee.Id);
                    employeeOtherInfoService.updateEmployeeOtherInfo(employeeModel.EmployeeOtherInfo);
                }


                return RedirectToAction("List");
            }
            else
            {
                var errors = ModelState.Select(x => x.Value.Errors)
                             .Where(y => y.Count > 0)
                             .ToList();
                return View();
            }
        }



        //Giriş yapan Employee ise 
        public IActionResult DetailEmployee()
        {
            int userID = CurrentUser.Id;
            var empInfo = service.getEmployeeUser(userID);
            return View(empInfo);
        }

        public IActionResult AddEmployee()
        {
            EmployeeDTO emp = new EmployeeDTO();
            return View(emp);
        }

        [HttpPost]
        public IActionResult AddEmployee(EmployeeDTO employee)
        {
            employee.CompanyID = (int)CurrentUser.CompanyID;
            service.newEmployee(employee);
            //employeeDetailService.newEmployeeDetail(newEmp.EmployeeDetail);

            return RedirectToAction("List");
        }

        public IActionResult DeleteEmployee(int Id)
        {
            service.deleteEmployee(Id);
            return RedirectToAction("List");
        }

        
    }
}