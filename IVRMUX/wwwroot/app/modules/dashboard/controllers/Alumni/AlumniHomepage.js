(function () {
    'use strict';
    angular
        .module('app')
        .controller('AlumniHomepageController1', AlumniHomepageController1)

    AlumniHomepageController1.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$q', '$cookieStore', '$stateParams', '$filter', 'FormSubmitter', '$window', 'superCache']
    function AlumniHomepageController1($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $q, $cookieStore, $stateParams, $filter, FormSubmitter, $window, superCache) {


      
        $scope.userPrivileges = "";
        var pageid = $stateParams.pageId;

        $scope.ddate = {};
        $scope.ddate = new Date();

        $scope.usrname = localStorage.getItem('username');
        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        if (admfigsettings.length > 0) {
            var logopath = admfigsettings[0].asC_Logo_Path;
        }

        $scope.imgname = logopath;

        var configsettings = JSON.parse(localStorage.getItem("feeconfigsettings"));

        $scope.currentPage = 1;
        $scope.itemsPerPages = 10;

        $scope.order = function (keyname) {
            $scope.sortKey = keyname;   //set the sortKey to the param passed
            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
        }
        //Loading the Intial Data Function
        $scope.loaddata = function () {
            apiService.getURI("AlumniMembership/get_intial_data", pageid).
                then(function (promise) {
                    if (promise.alumnitrue == false) {
                        $scope.adminslumni = true;
                        $scope.yearlst = promise.fillyear;
                        $scope.classlist = promise.fillclass;
                    }
                    else {
                        $scope.adminslumni = false;
                        $scope.amsT_ID = promise.studentDetails[0].almsT_Id;
                        $scope.studentname = promise.studentDetails[0].amsT_FirstName;
                        $scope.ALMST_AdmNo = promise.studentDetails[0].almsT_AdmNo;

                        $scope.AMST_JOIN_YEAR = promise.studentDetails[0].amsT_JOIN_YEAR;
                        $scope.AMST_JOIN_LEFT = promise.studentDetails[0].amsT_JOIN_LEFT;
                        $scope.ASMCL_Id_Join = promise.studentDetails[0].amsT_CLASS_JOIN;
                        $scope.ASMCL_Id_Left = promise.studentDetails[0].amsT_CLASS_LEFT;

                        $scope.ALMST_PhoneNo = promise.studentDetails[0].almsT_PhoneNo;
                        $scope.ALMST_PerStreet = promise.studentDetails[0].almsT_PerStreet;
                        $scope.ALMST_PerArea = promise.studentDetails[0].almsT_PerArea;

                        $scope.arrlist = promise.countryDrpDown;


                        $scope.ALMST_PerCountry = promise.studentDetails[0].ivrmmC_Id;
                       

                        $scope.arrStatelist2 = promise.statedropdown;

                        $scope.ALMST_PerState = promise.studentDetails[0].almsT_PerState;

                        $scope.ALMST_FatherName = promise.studentDetails[0].almsT_FatherName;
                        $scope.ALMST_DOB = promise.studentDetails[0].almsT_DOB;
                        $scope.ALMST_MobileNo = promise.studentDetails[0].almsT_MobileNo;
                        $scope.ALMST_emailId = promise.studentDetails[0].almsT_emailId;
                        //$scope.ALMST_Remarks = promise.studentDetails[0].almsT_Remarks;
                        $scope.ALMST_PerCity = promise.studentDetails[0].almsT_PerCity;
                        $scope.ALMST_PerPincode = promise.studentDetails[0].almsT_PerPincode;

                        $scope.ALMST_Marital_Status = promise.studentDetails[0].almsT_Marital_Status;

                        $scope.ALMST_Id = promise.ALMST_Id;


                        $scope.ALMST_PUC_QS_DETAILS = promise.saveddata[0].almsT_PUC_QS_DETAILS;
                        $scope.ALMST_PUC_INS_NAME = promise.saveddata[0].almsT_PUC_INS_NAME;
                        $scope.ALMST_PUC_PASSED_OUT = promise.saveddata[0].almsT_PUC_PASSED_OUT;
                        $scope.ALMST_PUC_PERCENTAGE = promise.saveddata[0].almsT_PUC_PERCENTAGE;
                        $scope.ALMST_PUC_PLACE = promise.saveddata[0].almsT_PUC_PLACE;
                        $scope.ALMST_PUC_STATE = promise.saveddata[0].almsT_PUC_STATE;

                        $scope.ALMST_UG_QS_DETAILS = promise.saveddata[0].almsT_UG_QS_DETAILS;
                        $scope.ALMST_UG_INS_NAME = promise.saveddata[0].almsT_UG_INS_NAME;
                        $scope.ALMST_UG_PASSED_OUT = promise.saveddata[0].almsT_UG_PASSED_OUT;
                        $scope.ALMST_UG_PERCENTAGE = promise.saveddata[0].almsT_UG_PERCENTAGE;

                        $scope.ALMST_UG_PLACE = promise.saveddata[0].almsT_UG_PLACE;
                        $scope.ALMST_UG_STATE = promise.saveddata[0].almsT_UG_STATE;
                        $scope.ALMST_PG_QS_DETAILS = promise.saveddata[0].almsT_PG_QS_DETAILS;
                        $scope.ALMST_PG_INS_NAME = promise.saveddata[0].almsT_PG_INS_NAME;

                        $scope.ALMST_PG_PASSED_OUT = promise.saveddata[0].almsT_PG_PASSED_OUT;
                        $scope.ALMST_PG_PERCENTAGE = promise.saveddata[0].almsT_PG_PERCENTAGE;
                        $scope.ALMST_PG_PLACE = promise.saveddata[0].almsT_PG_PLACE;
                        $scope.ALMST_PG_STATE = promise.saveddata[0].almsT_PG_STATE;

                        $scope.ALMST_ACH_DET = promise.saveddata[0].almsT_ACH_DET;
                        $scope.ALMST_ACH_REMARKS = promise.saveddata[0].almsT_ACH_REMARKS;
                        $scope.ALMST_PRO_COMPANY_NAME = promise.saveddata[0].almsT_PRO_COMPANY_NAME;
                        $scope.ALMST_PRO_DESIGNATION = promise.saveddata[0].almsT_PRO_DESIGNATION;

                        $scope.ALMST_PRO_OFFICE_NO = promise.saveddata[0].almsT_PRO_OFFICE_NO;
                        $scope.ALMST_PRO_ADDRESS = promise.saveddata[0].almsT_PRO_ADDRESS;
                        $scope.ALMST_PRO_REMARKS = promise.saveddata[0].almsT_PRO_REMARKS;
                    }
                   
                })
        }

        $scope.cleardata = function () {
            $state.reload();
        }

        $scope.clear = function () {

            $scope.ALMST_AdmNo = "";

            $scope.AMST_JOIN_YEAR = "";
            $scope.AMST_JOIN_LEFT = "";
            $scope.ASMCL_Id_Join = "";
            $scope.ASMCL_Id_Left = "";

            $scope.ALMST_PhoneNo = "";
            $scope.ALMST_PerStreet = "";
            $scope.ALMST_PerArea = "";

            $scope.arrlist = "";

            $scope.ALMST_PerCountry = "";

            $scope.arrStatelist2 = "";

            $scope.ALMST_PerState = "";

            $scope.ALMST_FatherName = "";
            $scope.ALMST_DOB = "";
            $scope.ALMST_MobileNo = "";
            $scope.ALMST_emailId = "";
            $scope.ALMST_Remarks = "";
            $scope.ALMST_PerCity = "";
            $scope.ALMST_PerPincode = "";

            $scope.ALMST_Marital_Status = "";

            $scope.ALMST_Id = "";


            $scope.ALMST_PUC_QS_DETAILS = "";
            $scope.ALMST_PUC_INS_NAME = "";
            $scope.ALMST_PUC_PASSED_OUT = "";
            $scope.ALMST_PUC_PERCENTAGE = "";
            $scope.ALMST_PUC_PLACE = "";
            $scope.ALMST_PUC_STATE = "";

            $scope.ALMST_UG_QS_DETAILS = "";
            $scope.ALMST_UG_INS_NAME = "";
            $scope.ALMST_UG_PASSED_OUT = "";
            $scope.ALMST_UG_PERCENTAGE = "";

            $scope.ALMST_UG_PLACE = "";
            $scope.ALMST_UG_STATE = "";
            $scope.ALMST_PG_QS_DETAILS = "";
            $scope.ALMST_PG_INS_NAME = "";

            $scope.ALMST_PG_PASSED_OUT = "";
            $scope.ALMST_PG_PERCENTAGE = "";
            $scope.ALMST_PG_PLACE = "";
            $scope.ALMST_PG_STATE = "";

            $scope.ALMST_ACH_DET = "";
            $scope.ALMST_ACH_REMARKS = "";
            $scope.ALMST_PRO_COMPANY_NAME = "";
            $scope.ALMST_PRO_DESIGNATION = "";

            $scope.ALMST_PRO_OFFICE_NO = "";
            $scope.ALMST_PRO_ADDRESS = "";
            $scope.ALMST_PRO_REMARKS = "";

        }

        //Onchange Of Academic Year
        $scope.onchangeacc = function (trmA_Id) {

            var data = {
                "ASMAY_Id": $scope.ASMAY_Id
            }
            apiService.create("AlumniMembership/Getstudentlist", data).
                then(function (promise) {
                    $scope.studentlst = promise.fillstudent;
                    if (promise.stu_name.length > 0) {
                        $scope.accchange = false;
                        $scope.stu_name = promise.stu_name;

                        $scope.clear();
                    }
                    else {

                        swal('No Records Found!!');
                        $scope.clear();
                    }

                })
        }



        //Student Search Dropdown

        //Onchange Of Academic Year for Approval
        $scope.onchangeaccapp = function (trmA_Id) {
            $scope.stu_name = {};
            var data = {
                "ASMAY_Id": $scope.ASMAY_Id
            }
            apiService.create("AlumniMembership/Getstudentlistapp", data).
                then(function (promise) {
                   
                    if (promise.stu_name.length > 0) {
                        //$scope.accchange = false;
                        $scope.stu_name = promise.stu_name;
                        $scope.ALSREG_Id = $scope.stu_name[0].alsreG_Id;
                    }
                    else {
                        $scope.stu_name = {};
                        $scope.ALSREG_Id = "";
                        swal('No Records Found!!');
                       
                       
                    }

                })
        }

        $scope.onchangecountry = function (ALMST_PerCountry) {
            var data = {
                "ALMST_PerCountry": ALMST_PerCountry
            }
            apiService.create("AlumniMembership/onchangecountry", data).
                then(function (promise) {
                    $scope.arrStatelist2 = promise.statedropdown;
                })
        }

        $scope.checkstudent = function (studentid) {
           
            var data = {
                "ALSREG_Id": studentid
            }
            apiService.create("AlumniMembership/checkstudent", data).
                then(function (promise) {

                    if (promise.studentDetails.length > 0) {
                        //$scope.accchange = false;
                        $scope.studentDetailscheck = promise.studentDetails;
                      
                    }
                    else {
                        $scope.studentDetailscheck = {};
                        swal('No Records Found!!');


                    }

                })
        }
        //Student Search Dropdown

        $scope.aproovedata = function (ALSREG_Id, arraystudent) {
                var data = {
                    "ALSREG_Id": ALSREG_Id,
                    "ALMST_Id": arraystudent[0].almsT_Id
                }

                apiService.create("AlumniMembership/aproovedata", data).
                    then(function (promise) {
                        if (promise.returnval == "True") {
                            swal("Alumni member successfully approved!!");
                            $state.reload();
                        }
                        else{
                            swal("Alumni member not approved!!");
                        }
                        
                        
                    })
        }


        $scope.searchfilter = function (objj) {

            if (objj.search.length >= '2') {

                var data = {
                    "searchfilter": objj.search,
                    "ASMAY_Id": $scope.ASMAY_Id
                }
                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                }

                apiService.create("AlumniMembership/searchfilter", data).
                    then(function (promise) {

                        $scope.studentlst = promise.fillstudent;

                        angular.forEach($scope.studentlst, function (objectt) {
                            if (objectt.amsT_FirstName.length > 0) {
                                var string = objectt.amsT_FirstName;
                                objectt.amsT_FirstName = string.replace(/  +/g, ' ');
                            }
                        })
                    })
            }

        }


        $scope.interacted = function (field) {

            return $scope.submitted;
        };
        //save

        $scope.saveData = function () {

            if ($scope.adminslumni == false) {
                $scope.amsT_ID = $scope.amsT_ID;
            }
            else {
                $scope.amsT_ID = $scope.Amst_Id.amsT_ID;
            }
           
            var data = {
                "ALMST_DOB": new Date($scope.ALMST_DOB),
                "ALMST_PUC_QS_DETAILS": $scope.ALMST_PUC_QS_DETAILS,
                "ALMST_PUC_INS_NAME": $scope.ALMST_PUC_INS_NAME,
                "ALMST_PUC_PASSED_OUT": $scope.ALMST_PUC_PASSED_OUT,
                "ALMST_PUC_PERCENTAGE": $scope.ALMST_PUC_PERCENTAGE,
                "ALMST_PUC_PLACE": $scope.ALMST_PUC_PLACE,
                "ALMST_PUC_STATE": $scope.ALMST_PUC_STATE,
                "ALMST_UG_QS_DETAILS": $scope.ALMST_UG_QS_DETAILS,
                "ALMST_UG_INS_NAME": $scope.ALMST_UG_INS_NAME,
                "ALMST_UG_PASSED_OUT": $scope.ALMST_UG_PASSED_OUT,
                "ALMST_UG_PERCENTAGE": $scope.ALMST_UG_PERCENTAGE,
                "ALMST_UG_PLACE": $scope.ALMST_UG_PLACE,
                "ALMST_UG_STATE": $scope.ALMST_UG_STATE,
                "ALMST_PG_QS_DETAILS": $scope.ALMST_PG_QS_DETAILS,
                "ALMST_PG_INS_NAME": $scope.ALMST_PG_INS_NAME,
                "ALMST_PG_PASSED_OUT": $scope.ALMST_PG_PASSED_OUT,
                "ALMST_PG_PERCENTAGE": $scope.ALMST_PG_PERCENTAGE,
                "ALMST_PG_PLACE": $scope.ALMST_PG_PLACE,
                "ALMST_PG_STATE": $scope.ALMST_PG_STATE,
                "ALMST_ACH_DET": $scope.ALMST_ACH_DET,
                "ALMST_ACH_REMARKS": $scope.ALMST_ACH_REMARKS,
                "ALMST_PRO_COMPANY_NAME": $scope.ALMST_PRO_COMPANY_NAME,
                "ALMST_PRO_DESIGNATION": $scope.ALMST_PRO_DESIGNATION,
                "ALMST_PRO_OFFICE_NO": $scope.ALMST_PRO_OFFICE_NO,
                "ALMST_PRO_ADDRESS": $scope.ALMST_PRO_ADDRESS,
                "ALMST_PRO_REMARKS": $scope.ALMST_PRO_REMARKS,
                "AMST_ID": $scope.amsT_ID,

                "ALMST_MobileNo": $scope.ALMST_MobileNo,
                "ALMST_PhoneNo": $scope.ALMST_PhoneNo,
                "ALMST_PerStreet": $scope.ALMST_PerStreet,
                //"ALMST_Remarks": $scope.ALMST_Remarks,
                "ALMST_emailId": $scope.ALMST_emailId,
                "ALMST_PerArea": $scope.ALMST_PerArea,
                "ALMST_PerCity": $scope.ALMST_PerCity,
                "ALMST_PerPincode": $scope.ALMST_PerPincode,
                "ALMST_Marital_Status": $scope.ALMST_Marital_Status,
                "ALMST_PerCountry": $scope.ALMST_PerCountry,
                "ALMST_PerState": $scope.ALMST_PerState
            }

            apiService.create("AlumniMembership/savedata", data).
                then(function (promise) {
                    swal(promise.returnval);
                    $state.reload();
                })
        }


        //-----student name selection change
        $scope.onchange = function (studentlst) {

            var studid = studentlst.amsT_ID;

            $scope.acc = true;
            $scope.accyer = $scope.ASMAY_Id

            var data = {
                "AMST_ID": studid,
                "ASMAY_Id": $scope.accyer
            }
            apiService.create("AlumniMembership/getstudata", data).
                then(function (promise) {

                    $scope.ALMST_AdmNo = promise.studentDetails[0].almsT_AdmNo;

                    $scope.AMST_JOIN_YEAR = promise.studentDetails[0].amsT_JOIN_YEAR;
                    $scope.AMST_JOIN_LEFT = promise.studentDetails[0].amsT_JOIN_LEFT;
                    $scope.ASMCL_Id_Join = promise.studentDetails[0].amsT_CLASS_JOIN;
                    $scope.ASMCL_Id_Left = promise.studentDetails[0].amsT_CLASS_LEFT;

                    $scope.ALMST_PhoneNo = promise.studentDetails[0].almsT_PhoneNo;
                    $scope.ALMST_PerStreet = promise.studentDetails[0].almsT_PerStreet;
                    $scope.ALMST_PerArea = promise.studentDetails[0].almsT_PerArea;

                    $scope.arrlist = promise.countryDrpDown;
                    $scope.ALMST_PerCountry = promise.studentDetails[0].ivrmmC_Id;
                    $scope.arrStatelist2 = promise.statedropdown;

                    $scope.ALMST_PerState = promise.almsT_PerState;
                   
                    $scope.ALMST_FatherName = promise.studentDetails[0].almsT_FatherName;
                    $scope.ALMST_DOB = promise.studentDetails[0].almsT_DOB;
                    $scope.ALMST_MobileNo = promise.studentDetails[0].almsT_MobileNo;
                    $scope.ALMST_emailId = promise.studentDetails[0].almsT_emailId;
                    //$scope.ALMST_Remarks = promise.studentDetails[0].almsT_Remarks;
                    $scope.ALMST_PerCity = promise.studentDetails[0].almsT_PerCity;
                    $scope.ALMST_PerPincode = promise.studentDetails[0].almsT_PerPincode;

                    $scope.ALMST_Marital_Status = promise.studentDetails[0].almsT_Marital_Status;

                    $scope.ALMST_Id = promise.ALMST_Id;


                    $scope.ALMST_PUC_QS_DETAILS = promise.saveddata[0].almsT_PUC_QS_DETAILS;
                    $scope.ALMST_PUC_INS_NAME = promise.saveddata[0].almsT_PUC_INS_NAME;
                    $scope.ALMST_PUC_PASSED_OUT = promise.saveddata[0].almsT_PUC_PASSED_OUT;
                    $scope.ALMST_PUC_PERCENTAGE = promise.saveddata[0].almsT_PUC_PERCENTAGE;
                    $scope.ALMST_PUC_PLACE = promise.saveddata[0].almsT_PUC_PLACE;
                    $scope.ALMST_PUC_STATE = promise.saveddata[0].almsT_PUC_STATE;

                    $scope.ALMST_UG_QS_DETAILS = promise.saveddata[0].almsT_UG_QS_DETAILS;
                    $scope.ALMST_UG_INS_NAME = promise.saveddata[0].almsT_UG_INS_NAME;
                    $scope.ALMST_UG_PASSED_OUT = promise.saveddata[0].almsT_UG_PASSED_OUT;
                    $scope.ALMST_UG_PERCENTAGE = promise.saveddata[0].almsT_UG_PERCENTAGE;

                    $scope.ALMST_UG_PLACE = promise.saveddata[0].almsT_UG_PLACE;
                    $scope.ALMST_UG_STATE = promise.saveddata[0].almsT_UG_STATE;
                    $scope.ALMST_PG_QS_DETAILS = promise.saveddata[0].almsT_PG_QS_DETAILS;
                    $scope.ALMST_PG_INS_NAME = promise.saveddata[0].almsT_PG_INS_NAME;

                    $scope.ALMST_PG_PASSED_OUT = promise.saveddata[0].almsT_PG_PASSED_OUT;
                    $scope.ALMST_PG_PERCENTAGE = promise.saveddata[0].almsT_PG_PERCENTAGE;
                    $scope.ALMST_PG_PLACE = promise.saveddata[0].almsT_PG_PLACE;
                    $scope.ALMST_PG_STATE = promise.saveddata[0].almsT_PG_STATE;

                    $scope.ALMST_ACH_DET = promise.saveddata[0].almsT_ACH_DET;
                    $scope.ALMST_ACH_REMARKS = promise.saveddata[0].almsT_ACH_REMARKS;
                    $scope.ALMST_PRO_COMPANY_NAME = promise.saveddata[0].almsT_PRO_COMPANY_NAME;
                    $scope.ALMST_PRO_DESIGNATION = promise.saveddata[0].almsT_PRO_DESIGNATION;

                    $scope.ALMST_PRO_OFFICE_NO = promise.saveddata[0].almsT_PRO_OFFICE_NO;
                    $scope.ALMST_PRO_ADDRESS = promise.saveddata[0].almsT_PRO_ADDRESS;
                    $scope.ALMST_PRO_REMARKS = promise.saveddata[0].almsT_PRO_REMARKS;

                })
        }

    } }) ();