(function () {
    'use strict';
    angular.module('app').controller('AddVisitorsController', AddVisitorsController)
    AddVisitorsController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'Excel', '$timeout', '$filter', 'superCache', 'myFactoryvisitor']
    function AddVisitorsController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, Excel, $timeout, $filter, superCache, myFactoryvisitor) {

        $scope.searchValue = "";
        $scope.currentPage = 1;
        $scope.itemsPerPage = 10;
        $scope.AMVM_Id = 0;
        $scope.obj1 = {};
        $scope.obsupdate = {};
        $scope.VMAP_Id = 0;
        $scope.edit = false;
        $scope.TriggerOTP = false;
        $scope.plMaxdate = new Date();
        $scope.VMMV_ExitDate = new Date();
        $scope.VMMV_MeetingDateTime = new Date();
        $scope.plMaxdate.setDate($scope.plMaxdate.getDate());
        $scope.sort = function (key) {
            $scope.sortReverse = ($scope.sortKey === key) ? !$scope.sortReverse : $scope.sortReverse;
            $scope.sortKey = key;
        };

        var miid = myFactoryvisitor.get();
        if (miid > 0) {
            $scope.mI_Id = miid;
        } else {
            $scope.mI_Id = 0;
        }

        $scope.radiomodal = 'int_dept';
        $scope.loadgrid = function () {
            $scope.vis_list = [];
            $scope.gridoptions = [];
            $scope.radiomodal = 'int_dept';

            if ($scope.mI_Id === undefined || $scope.mI_Id === null || $scope.mI_Id === '') {
                $scope.mI_Id = 0;
            }

            apiService.getURI("AddVisitors/getDetails/", $scope.mI_Id).then(function (promise) {
                $scope.mI_Id = promise.mI_Id;
                $scope.institutionlist = promise.institutionlist;
                if (promise.gridoptions !== null && promise.gridoptions.length > 0) {
                    $scope.gridoptions = promise.gridoptions;
                    $scope.presentCountgrid = promise.gridoptions.length;
                }
                $scope.emplist = promise.emplist;
                $scope.emplistautjorizedby = promise.emplistautjorizedby;
                $scope.visitorList = promise.visitorList
            });


        };

        $scope.hstep = 1;
        $scope.mstep = 1;

        $scope.ismeridian = true;
        $scope.toggleMode = function () {
            $scope.ismeridian = !$scope.ismeridian;
        };

        $scope.screport = false;
        $scope.submitted = false;

        $scope.saveData = function (obj1s) {
            var obj = {};

            if ($scope.myForm.$valid) {
                var deptflag = false;
                var VMMV_PersonToMeet = "";
                var VMMV_AuthorisationBy = "";
                var tomeethrmeid = 0;
                var toathurizedid = 0;
                var totenantmeet = 0;

                if ($scope.radiomodal === 'int_dept') {
                    deptflag = false;
                    VMMV_AuthorisationBy = "";
                    VMMV_PersonToMeet = "";
                    toathurizedid = obj1s.ToAuthHRMEId.hrmE_Id;

                    if (tomeethrmeid = obj1s.ToMeetHRMEId.vmaP_HRME_Id > 0) {
                        tomeethrmeid = obj1s.ToMeetHRMEId.vmaP_HRME_Id;
                    }
                    else {
                        tomeethrmeid = obj1s.ToMeetHRMEId.hrmE_Id;
                    }

                } else if ($scope.radiomodal === 'tenentvisitors') {

                    totenantmeet = $scope.obj1.ToMeetTenenet.tenantagR_Id
                } else {
                    deptflag = true;
                    VMMV_AuthorisationBy = obj1s.VMMV_AuthorisationBy;
                    VMMV_PersonToMeet = obj1s.VMMV_PersonToMeet;
                    tomeethrmeid = 0;
                    toathurizedid = 0;
                }

                obj = {
                    "VMMV_Id": $scope.VMMV_Id,
                    "VMMV_VisitorName": $scope.VMMV_VisitorName,
                    "VMMV_VisitorContactNo": $scope.vmmV_VisitorContactNo,
                    "VMMV_VisitorEmailid": $scope.VMMV_VisitorEmailid,
                    "VMMV_IdentityCardType": $scope.VMMV_IdentityCardType,
                    "VMMV_CardNo": $scope.VMMV_CardNo,
                    "VMMV_VisitTypeFlg": $scope.VMMV_VisitTypeFlg,
                    "VMMV_FromPlace": $scope.VMMV_FromPlace,
                    "VMMV_MeetingDuration": $scope.VMMV_MeetingDuration,
                    "VMMV_MeetingDateTime": new Date($scope.VMMV_MeetingDateTime).toDateString(),
                    "VMMV_EntryDateTime": $filter('date')($scope.VMMV_EntryDateTime, "HH:mm"),
                    "VMMV_MeetingLocation": $scope.VMMV_MeetingLocation,
                    "VMMV_MeetingPurpose": $scope.VMMV_MeetingPurpose,
                    "VMMV_PersonsAccompanying": $scope.VMMV_PersonsAccompanying,
                    "VMMV_FromAddress": $scope.VMMV_FromAddress,
                    "VMVTMT_ToMeet_HRME_Id": tomeethrmeid,
                    "VMMV_Remarks": $scope.VMMV_Remarks,
                    "VMMV_VIPFlg": $scope.VMMV_VIPFlg,
                    "dept_flag": deptflag,
                    "MI_Id": $scope.mI_Id,
                    "VMMV_PersonToMeet": VMMV_PersonToMeet,
                    "VMMV_AuthorisationBy": VMMV_AuthorisationBy,
                    "HRME_Id": toathurizedid,
                    "VMAP_Id": $scope.VMAP_Id,
                    "TENANTAGR_Id": totenantmeet,
                    "VMFlag": "VM",
                    multivisitor: $scope.morevisitor,

                };
                apiService.create("AddVisitors/saveData", obj).then(function (promise) {
                    $scope.image = promise.ascisS_QRCode;
                    //$scope.image = promise.qrCodeImage

                    if (promise.returnVal === 'saved') {
                        swal("Record Saved Successfully by sending SMS and Email");
                        $scope.vis_list = promise.vis_list;
                        $scope.screport = true;
                        $scope.SchoolLogo = promise.institution[0].mI_Logo;
                        $scope.SchollName = promise.institution[0].mI_Name;
                        $scope.SchollAdd = promise.institution[0].mI_Address1;
                        // $state.reload();
                    }
                    else if (promise.returnVal === 'updated') {
                        swal("Record Updated Successfully");
                        $scope.vis_list = promise.vis_list;
                        $scope.screport = true;
                        $scope.SchoolLogo = promise.institution[0].mI_Logo;
                        $scope.SchollName = promise.institution[0].mI_Name;
                        $scope.SchollAdd = promise.institution[0].mI_Address1;
                        //$state.reload();
                    }
                    else if (promise.returnVal === 'duplicate') {
                        swal("Record already exist");
                    }
                    else if (promise.returnVal === "savingFailed") {
                        swal("Failed to save record");
                    }
                    else if (promise.returnVal === "updateFailed") {
                        swal("Failed to update record");
                    }
                    else {
                        swal("Sorry...something went wrong");
                    }
                });
            }
            else {
                $scope.submitted = true;
            }
            $scope.save();

        };

        $scope.save = function () {
            //var html = "";
            $scope.Certificate = "TC";
            $scope.qrcode = "";

          
           
        
          
            var Template1 = document.getElementById("printSectionId").innerHTML;
            var data = {
                "VMMV_Id": $scope.VMMV_Id,
                "ASCISS_HtmlTemplate": Template1,
                "ASCISS_CertificateFlg": "VM",
               "ASCISS_QRCode": $scope.ASCISS_QRCode
            };

            apiService.create("AddVisitors/QRCODECertificates", data).then(function (promise) {

                $scope.image = promise.ascisS_QRCode;


                if (response.data) {
                    $scope.VMMV_Id = response.data.VMMV_Id;

                    console.log("Saved successfully, VMMV_Id: ", $scope.VMMV_Id);
                } else {
                    console.error("Error saving visitor data.");
                }



            });
        };


        // ***************** GET OTP ****************** //
        $scope.getotp = function () {
            var remarks = "";
            if ($scope.gphS_Remarks !== undefined && $scope.gphS_Remarks !== null) {
                remarks = $scope.gphS_Remarks;
            } else {
                remarks = "";
            }
            var data = {
                "VMMV_VisitorEmailid": $scope.VMMV_VisitorEmailid,
                "VMMV_VisitorContactNo": $scope.vmmV_VisitorContactNo,
            }

            apiService.create("AddVisitors/getotp", data).then(function (promise) {
                if (promise !== null) {
                    if (promise.message !== null && promise.message !== "") {
                        swal(promise.message);

                        $('#otpverification').modal('show');

                    }
                }

            });
        };


        $scope.otpsucess = false;
        $scope.submitted2 = false;
        $scope.GPHS_OTP = "";
        $scope.getnewotpverification = function (user) {


            $scope.newotpsave = $scope.GPHS_OTP;
            var data = {
                "genotp": $scope.GPHS_OTP,
            }

            apiService.create("AddVisitors/getnewotpverification", data).then(function (promise) {

                if (promise.returnval == true) {
                    swal('Your OTP Verification Is Success');
                    $state.reload();
                }
                else {
                    if (promise.message === "Success") {
                        $scope.otpsucess = true;
                        $scope.TriggerOTP = false;
                        swal(promise.message);
                        $('#otpverification').modal('hide');
                    } else {
                        swal(promise.message);
                        $scope.GPHS_OTP = "";
                    }
                }
            })

        }

        $scope.fiedempty = function () {
            $scope.VMMV_VisitorName = "";
            $scope.VMMV_VisitorContactNo = "";
            $scope.VMMV_VisitorEmailid = "";
            $scope.VMMV_IdentityCardType = "";
            $scope.VMMV_CardNo = "";
            $scope.VMMV_VisitTypeFlg = "";
            $scope.VMMV_FromPlace = "";
            $scope.VMMV_MeetingDuration = "";
            $scope.VMMV_MeetingDateTime = "";
            $scope.VMMV_EntryDateTime = "";
            $scope.VMMV_MeetingLocation = "";
            $scope.VMMV_MeetingPurpose = "";
            $scope.obj1.VMMV_AuthorisationBy = "";
            $scope.VMMV_PersonsAccompanying = "";
            $scope.VMMV_FromAddress = "";
            $scope.emp1 = "";
            $scope.VMMV_Remarks = "";
            $scope.radiomodal = 'int_dept';
            $scope.meetpersion = "";
            $scope.submitted = false;
        };

        $scope.morevisitor = [{ id: 'viss1' }];
        $scope.addvisitor = function () {
            var newItemNo = $scope.morevisitor.length + 1;
            if (newItemNo <= 10) {
                $scope.morevisitor.push({ 'id': 'viss1' + newItemNo });
            }
        };

        $scope.removeaddvisitor = function (index) {
            var newItemNo = $scope.morevisitor.length - 1;
            $scope.morevisitor.splice(index, 1);

            if ($scope.morevisitor.length === 0) {
                //data
            }
        };

        $scope.editdata = function (data) {
            var obj = {
                "VMMV_Id": data.vmmV_Id,
                "MI_Id": $scope.mI_Id
            };
            apiService.create("AddVisitors/Edit/", obj).then(function (promise) {

                $scope.editDetails = promise.editDetails;
                $scope.mI_Id = promise.editDetails[0].mI_Id;
                $scope.VMMV_Id = promise.editDetails[0].vmmV_Id;
                $scope.VMMV_VisitorName = promise.editDetails[0].vmmV_VisitorName;
                $scope.vmmV_VisitorContactNo = promise.editDetails[0].vmmV_VisitorContactNo;
                $scope.VMMV_VisitorEmailid = promise.editDetails[0].vmmV_VisitorEmailid;
                $scope.VMMV_IdentityCardType = promise.editDetails[0].vmmV_IdentityCardType;
                $scope.VMMV_CardNo = promise.editDetails[0].vmmV_CardNo;
                $scope.VMMV_VisitTypeFlg = promise.editDetails[0].vmmV_VisitTypeFlg;
                $scope.VMMV_VIPFlg = promise.editDetails[0].vmmV_VIPFlg;
                $scope.VMMV_FromPlace = promise.editDetails[0].vmmV_FromPlace;
                $scope.VMMV_MeetingDuration = promise.editDetails[0].vmmV_MeetingDuration;
                $scope.VMMV_MeetingDateTime = new Date(promise.editDetails[0].vmmV_MeetingDateTime);
                $scope.VMMV_EntryDateTime = moment(promise.editDetails[0].vmmV_EntryDateTime, 'HH:mm').format();
                $scope.VMMV_MeetingLocation = promise.editDetails[0].vmmV_MeetingLocation;
                $scope.VMMV_MeetingPurpose = promise.editDetails[0].vmmV_MeetingPurpose;
                $scope.obj1.VMMV_AuthorisationBy = promise.editDetails[0].vmmV_AuthorisationBy;
                $scope.VMMV_PersonsAccompanying = promise.editDetails[0].vmmV_PersonsAccompanying;
                $scope.VMMV_FromAddress = promise.editDetails[0].vmmV_FromAddress;
                $scope.VMMV_Remarks = promise.editDetails[0].vmmV_Remarks;
                $scope.dept_flag = promise.editDetails[0].vmmV_DeptFlag;

                $scope.morevisitor = promise.getmulti_visitors;
                $scope.edit = true;
                if ($scope.dept_flag === true && $scope.radiomodal == 'out_dept') {

                    $scope.obj1.VMMV_PersonToMeet = promise.editDetails[0].vmmV_PersonToMeet;
                }
                else if ($scope.radiomodal == 'int_dept') {

                    $scope.radiomodal = 'int_dept';
                    if (promise.editDetails[0].vmmV_ToMeet !== null || promise.editDetails[0].vmmV_ToMeet !== 0) {
                        $scope.emp1 = promise.editDetails[0];
                        $scope.obj1.ToMeetHRMEId = promise.editemplist[0];
                        $scope.obj1.ToAuthHRMEId = promise.editemplistauthby[0];
                        $scope.emp1.hrmE_Id = promise.editemplist[0].hrmE_Id;
                        $scope.emp12.hrmE_Id = promise.editemplistauthby[0].hrmE_Id;
                        $scope.emplist = promise.emplist;
                        $scope.emplistautjorizedby = promise.emplistautjorizedby;

                    }

                }
                else {
                    if ($scope.radiomodal = 'tenentvisitors') {
                        $scope.radiomodal = 'tenentvisitors'
                        $scope.emp1 = promise.editDetails[0];
                        $scope.obj1.ToMeetHRMEId = promise.editemplist[0];
                        $scope.obj1.ToAuthHRMEId = promise.editemplistauthby[0];
                        $scope.emp1.hrmE_Id = promise.editemplist[0].hrmE_Id;
                        $scope.emp12.hrmE_Id = promise.editemplistauthby[0].hrmE_Id;
                        $scope.emplist = promise.emplist;
                        $scope.emplistautjorizedby = promise.emplistautjorizedby;

                    }
                }

            });
        };

        $scope.deactive = function (newuser1, SweetAlertt) {
            var mgs = "";
            if (newuser1.amvM_ActiveFlag === false) {
                mgs = "Activate";
            } else {
                mgs = "Deactivate";
            }
            swal({
                title: "Are you sure?",
                text: "Do you want to  " + mgs + " the Record?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55",
                confirmButtonText: "Yes, " + mgs + " it!",
                cancelButtonText: "Cancel..!",
                closeOnConfirm: false,
                closeOnCancel: false
            },
                function (isConfirm) {
                    if (isConfirm) {
                        apiService.create("AddVisitors/deactivate", newuser1).then(function (promise) {
                            if (promise.value === true) {
                                if (promise.msg !== null) {
                                    swal(promise.msg);
                                    $scope.loadgrid();
                                }
                            }
                            else {
                                swal('Failed to Activate/Deactivate the Record');
                            }
                        });
                    } else {
                        swal("Cancelled");
                    }
                });
        };

        $scope.onChangeradio = function (radiomodal) {
            $scope.VMMV_VisitorName = "";
            $scope.VMMV_VisitorContactNo = "";
            $scope.VMMV_VisitorEmailid = "";
            $scope.VMMV_IdentityCardType = "";
            $scope.VMMV_CardNo = "";
            $scope.VMMV_VisitTypeFlg = "";
            $scope.VMMV_VIPFlg = "";
            $scope.VMMV_FromPlace = "";
            $scope.VMMV_MeetingDuration = "";
            $scope.VMMV_MeetingDateTime = "";
            $scope.VMMV_EntryDateTime = "";
            $scope.VMMV_MeetingLocation = "";
            $scope.VMMV_MeetingPurpose = "";
            $scope.VMMV_AuthorisationBy = "";
            $scope.VMMV_PersonsAccompanying = "";
            $scope.VMMV_FromAddress = "";
            $scope.VMMV_Remarks = "";
            $scope.vmmV_VisitorContactNo = "";
            $scope.gridoptions = [];
            $scope.vis_list = [];
            $scope.obj1 = {};
            if (radiomodal === 'int_dept') {
                $scope.dept_flag = 0;
                $scope.type = '0';
            }
            else if (radiomodal === 'out_dept') {
                $scope.dept_flag = 1;
                $scope.type = '1';
            }
            else if (radiomodal === 'tenentvisitors') {
                // $scope.dept_flag = 2;
                $scope.type = '2';
            }
            var data = {
                "dept_flag": $scope.dept_flag,
                "type": $scope.type,
                "MI_Id": $scope.mI_Id
            };
            apiService.create("AddVisitors/changeofradio", data).then(function (promise) {
                if (promise.gridoptions.length > 0) {
                    $scope.gridoptions = promise.gridoptions;
                }
                if (promise.visiteSerchList.length > 0 && promise.visiteSerchList != null) {
                    $scope.visiteSerchList = promise.visiteSerchList;
                }
                if (promise.buildinglist.length > 0 && promise.buildinglist != null) {
                    $scope.buildinglist = promise.buildinglist;
                }
                if (promise.roomlist.length > 0 && promise.roomlist != null) {
                    $scope.roomlist = promise.roomlist;
                }
            });
        };

        $scope.get_roomdetails = function (objs) {
            $scope.roomlistdetails = [];
            angular.forEach($scope.roomlist, function (dd) {
                if (dd.hrmB_Id == objs.buildingname.hrmB_Id) {

                    $scope.roomlistdetails.push({
                        hrmR_RoomName: dd.hrmR_RoomName,
                        hrmR_Id: dd.hrmR_Id,

                    });
                }
            });

        };




        $scope.get_tenantdetails = function (objs) {
            $scope.tenantdetails = [];
            angular.forEach($scope.visiteSerchList, function (dd) {
                if (dd.hrmB_Id == objs.buildingname.hrmB_Id && dd.hrmR_Id == objs.roomname.hrmR_Id) {

                    $scope.tenantdetails.push({
                        tenantagR_TenantName: dd.tenantagR_TenantName,
                        tenantagR_Id: dd.tenantagR_Id,

                    });
                }
            });

        };



        $scope.GetVisitorDetail = function (objs) {
            $scope.visitorsname = "";
            $scope.VisitorAdress = "";
            $scope.EmailId = "";
            $scope.ContactNum = "";
            $scope.submitted1 = false;
            var data = {
                "VMMV_Id": objs.vmmV_Id,
                "VMMVVI_Id": $scope.vmmvvI_Id
            };
            apiService.create("AddVisitors/Getmultivisitordetails", data).then(function (promise) {
                if (promise !== null) {
                    $scope.getmultivisitordetails = promise.extravisitor;
                }
            });
        };

        $scope.GetVisitorDetails = function (objs) {
            $scope.obsupdate.VMMV_ExitDateTimes = "";
            $scope.VMMV_ExitDate = new Date();
            $scope.MeetingDateTime = null;
            $scope.visitorname = "";
            $scope.sempname = "";
            $scope.VMMV_Id_New = "";
            $scope.maxDate1 = "";
            $scope.minDate1 = "";
            $scope.submitted1 = false;

            var data = {
                "VMMV_Id": objs.vmmV_Id,
                "MI_Id": $scope.mI_Id,
                "VMMVVI_Id": $scope.vmmvvI_Id
            };
            apiService.create("AddVisitors/GetVisitorDetails", data).then(function (promise) {
                if (promise !== null) {
                    $scope.viewdetails = promise.viewdetails;
                    $scope.visitorname = $scope.viewdetails[0].vmmV_VisitorName;
                    $scope.MeetingDateTime = new Date($scope.viewdetails[0].vmmV_MeetingDateTime);
                    $scope.VMMV_Remarkss = $scope.viewdetails[0].vmmV_Remarks;
                    $scope.EntryDateTime = $scope.viewdetails[0].vmmV_EntryDateTime;
                    $scope.sempname = $scope.viewdetails[0].empname;
                    $scope.VMMV_Id_New = $scope.viewdetails[0].vmmV_Id;
                    $scope.obsupdate.VMMV_ExitDateTimes = moment($scope.viewdetails[0].vmmV_ExitDateTime, 'HH:mm').format();
                    if ($scope.viewdetails[0].vmmV_ExitDate !== null) {
                        $scope.VMMV_ExitDate = new Date($scope.viewdetails[0].vmmV_ExitDate);
                    }
                    $scope.maxDate1 = new Date();
                    $scope.minDate1 = new Date($scope.viewdetails[0].vmmV_ExitDate);

                    $scope.Mintime = moment($scope.EntryDateTime, 'HH:mm').format();
                }
            });
        };

        $scope.GetVisitorAssginDetails = function (objs) {
            var data = {
                "VMMV_Id": objs.vmmV_Id,
                "MI_Id": $scope.mI_Id
            };
            apiService.create("AddVisitors/GetVisitorAssginDetails", data).then(function (promise) {
                if (promise !== null) {
                    $scope.visitorassigndetails = promise.visitorassigndetails;

                }
            });
        };

        $scope.UpdateStatus = function (objs) {
            if (objs.VMMV_ExitDateTimes === undefined || objs.VMMV_ExitDateTimes === null || objs.VMMV_ExitDateTimes === "") {
                swal("Select Exit Time");
                return;
            }
            if ($scope.myForm1.$valid) {
                var data = {
                    "VMMV_Id": $scope.VMMV_Id_New,
                    "MI_Id": $scope.mI_Id,
                    "VMMV_ExitDateTime": $filter('date')(objs.VMMV_ExitDateTimes, "HH:mm"),
                    "VMMV_Remarks": $scope.VMMV_Remarkss,
                    "VMMV_ExitDate": new Date($scope.VMMV_ExitDate).toDateString()
                };
                apiService.create("AddVisitors/UpdateStatus", data).then(function (promise) {
                    if (promise.returnVal === 'updated') {
                        swal("Status Updated Successfully");
                        $('#mymodalviewdetailsfirsttab').modal('hide');

                    }
                });
            } else {
                $scope.submitted1 = false;
                //$state.reload();
            }
        };

        //$scope.onSelectlibrary = function () {
        //    $scope.loadgrid();
        //}


        // Previous Visitor Details
        $scope.searchfilters = function (objj) {
            if (objj.search.length >= '2') {
                var data = {
                    "MI_Id": $scope.mI_Id,
                    "searchfilter": objj.search,
                    // "VMMV_VisitorContactNo": objj.search,
                    "searchtype": $scope.searchtype,
                };
                apiService.create("AddVisitors/searchfilter", data).then(function (promise) {
                    if (promise !== null) {
                        $scope.visitorList = promise.visitorList;
                    }
                });
            }
        };

        $scope.visitornameschangeddetails = function () {
            var obj = {
                "VMMV_Id": $scope.vmmV_Id.vmmV_Id,
                "MI_Id": $scope.mI_Id
            }
            apiService.create("AddVisitors/visitornameschange/", obj).then(function (promise) {
                $scope.visitornameschange = promise.visitornameschange;
                $scope.VMMV_VisitorName = promise.visitornameschange[0].vmmV_VisitorName;
                $scope.vmmV_VisitorContactNo = promise.visitornameschange[0].vmmV_VisitorContactNo;
                $scope.VMMV_VisitorEmailid = promise.visitornameschange[0].vmmV_VisitorEmailid;
                $scope.VMMV_CardNo = promise.visitornameschange[0].vmmV_CardNo;
                $scope.VMMV_FromPlace = promise.visitornameschange[0].vmmV_FromPlace;
                $scope.VMMV_FromAddress = promise.visitornameschange[0].vmmV_FromAddress;
                $scope.VMMV_MeetingDuration = promise.visitornameschange[0].vmmV_MeetingDuration;
                $scope.VMMV_MeetingLocation = promise.visitornameschange[0].vmmV_MeetingLocation;
                $scope.VMMV_MeetingDateTime = new Date(promise.visitornameschange[0].vmmV_MeetingDateTime);
                $scope.VMMV_EntryDateTime = new moment(promise.visitornameschange[0].vmmV_EntryDateTime, 'HH:mm').format();
                $scope.VMMV_MeetingPurpose = promise.visitornameschange[0].vmmV_MeetingPurpose;
                $scope.VMMV_VisitorEmailid = promise.visitornameschange[0].vmmV_VisitorEmailid;

            });
        };



        $scope.clearsearch = function () {
            $scope.visitorList = [];
            $scope.vmmV_Id.vmmV_Id = "";
        }

        $scope.SearchAppVisitors = function () {
            $scope.appvisitorname = "";
            $scope.appvisitoremailid = "";
            $scope.appvisitortomeet = "";
            $scope.appvisitormeetingdatetime = "";

            $scope.appvisitorcontactno = "";
            $scope.appvisitorfromplace = "";
            $scope.appvisitormettingpurpose = "";
            $scope.appvisitorremarks = "";
            $scope.getappvistiordetails = [];
            $scope.visitorList = [];
            $scope.VMAP_Id = "";
            $scope.VMAPNew_Id = "";
            $scope.appvisitorstatus = "";
            var data = {
                "MI_Id": $scope.mI_Id
            };
            apiService.create("AddVisitors/SearchAppVisitors", data).then(function (promise) {
                if (promise !== null) {
                    $scope.getappvistiordetails = promise.getappvistiordetails;
                    if ($scope.getappvistiordetails === null || $scope.getappvistiordetails.length === 0) {
                        swal("No Visitors Found");
                    } else {
                        $('#myModalreadmit').modal('show');
                    }
                }
            });
        };

        $scope.OnchangeVisitorname = function () {
            angular.forEach($scope.getappvistiordetails, function (dd) {
                if (dd.vmaP_Id === parseInt($scope.VMAPNew_Id.vmaP_Id)) {
                    $scope.appvisitorname = dd.vmaP_VisitorName;
                    $scope.appvisitoremailid = dd.vmaP_VisitorEmailid;
                    $scope.appvisitortomeet = dd.vmaP_ToMeet;
                    $scope.appvisitormeetingdatetime = new Date(dd.vmaP_MeetingDateTime);

                    $scope.appvisitorcontactno = dd.vmaP_VisitorContactNo;
                    $scope.appvisitorfromplace = dd.vmaP_FromPlace;
                    $scope.appvisitormettingpurpose = dd.vmaP_MeetingPurpose;
                    $scope.appvisitorremarks = dd.vmaP_Remarks;
                    $scope.appvisitorstatus = dd.vmaP_Status;
                    $scope.VMAP_Id = dd.vmaP_Id;
                }
            });
        };

        $scope.addtocart = function () {
            if ($scope.myForm2.$valid) {
                $scope.VMMV_Id = 0;

                var data = {
                    "VMAP_Id": $scope.VMAPNew_Id.vmaP_Id
                };

                apiService.create("AddVisitors/addtocart", data).then(function (promise) {

                    if (promise !== null) {
                        $scope.getAppointmentdetails = promise.getAppointmentdetails;
                        $scope.getAppointment_visitordetails = promise.getAppointment_visitordetails;

                        angular.forEach($scope.emplist, function (dd) {
                            if (dd.hrmE_Id === $scope.getAppointmentdetails[0].vmaP_HRME_Id) {
                                $scope.getAppointmentdetails[0].hrmE_EmployeeFirstName = dd.hrmE_EmployeeFirstName;
                            }
                        });



                        $scope.morevisitor = [];

                        if ($scope.getAppointment_visitordetails !== null && $scope.getAppointment_visitordetails.length > 0) {
                            angular.forEach($scope.getAppointment_visitordetails, function (dd) {
                                $scope.morevisitor.push({
                                    vmmvvI_VisitorName: dd.vmapvI_VisitorName, vmmvvI_VisitorContactNo: dd.vmapvI_VisitorContactNo,
                                    vmmvvI_VisitorEmailId: dd.vmapvI_VisitorEmailId, vmmvvI_VisitorCardNo: '',
                                    vmmvvI_VisitorAddress: dd.vmapvI_VisitorAddress, vmmvvI_Remarks: dd.vmapvI_Remarks
                                });
                            });
                        } else {
                            $scope.morevisitor = [{ id: 'viss1' }];
                        }

                        $scope.VMMV_VisitorName = $scope.getAppointmentdetails[0].vmaP_VisitorName;
                        $scope.vmmV_VisitorContactNo = $scope.getAppointmentdetails[0].vmaP_VisitorContactNo;
                        $scope.VMMV_VisitorEmailid = $scope.getAppointmentdetails[0].vmaP_VisitorEmailid;
                        $scope.VMMV_VisitTypeFlg = $scope.getAppointmentdetails[0].vmaP_VisitTypeFlg;
                        $scope.VMMV_MeetingLocation = $scope.getAppointmentdetails[0].vmaP_MeetingLocation;
                        $scope.VMMV_MeetingDuration = $scope.getAppointmentdetails[0].vmaP_MeetingDuration;
                        $scope.VMMV_FromPlace = $scope.getAppointmentdetails[0].vmaP_FromPlace;

                        if ($scope.getAppointmentdetails[0].vmaP_MeetingDateTime !== null) {
                            $scope.VMMV_MeetingDateTime = new Date($scope.getAppointmentdetails[0].vmaP_MeetingDateTime);
                            $scope.VMMV_EntryDateTime = moment($scope.getAppointmentdetails[0].vmaP_MeetingTiming, 'HH:mm').format();
                        }
                        $scope.VMMV_MeetingPurpose = $scope.getAppointmentdetails[0].vmaP_MeetingPurpose;
                        $scope.VMMV_FromAddress = $scope.getAppointmentdetails[0].vmaP_FromAddress;
                        $scope.VMMV_PersonsAccompanying = $scope.getAppointmentdetails[0].vmaP_PersonsAccompanying;
                        $scope.VMMV_Remarks = $scope.getAppointmentdetails[0].vmaP_Remarks;
                        $scope.VMAP_Id = $scope.getAppointmentdetails[0].vmaP_Id;

                        $scope.obj1.ToMeetHRMEId = $scope.getAppointmentdetails[0];

                        $('#myModalreadmit').modal('hide');
                    }
                });
            } else {
                $scope.submitted2 = true;
            }
        };

        $scope.removeall = function () {
            $scope.appvisitorname = "";
            $scope.appvisitoremailid = "";
            $scope.appvisitortomeet = "";
            $scope.appvisitormeetingdatetime = "";

            $scope.appvisitorcontactno = "";
            $scope.appvisitorfromplace = "";
            $scope.appvisitormettingpurpose = "";
            $scope.appvisitorremarks = "";
            $scope.getappvistiordetails = [];
            $scope.VMAP_Id = "";
            $scope.appvisitorstatus = "";
            $('#myModalreadmit').modal('hide');
        };

        $scope.cancel = function () {
            $state.reload();
        };

        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        $scope.interacted1 = function (field) {
            return $scope.submitted1;
        };

        $scope.interacted2 = function (field) {
            return $scope.submitted2;
        };

        $scope.Print = function () {
            var innerContents = '';
            innerContents = document.getElementById("printSectionId").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/printpdf.css" />' +
                '<link type="text/css" media="print" href="css/print/preadmission/AdmissionReports.css" rel="stylesheet" />' +
                '</head> <body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>'
            );
            popupWinindow.document.close();
        };

        $scope.searchchkbx = "";
        $scope.filterchkbx = function (obj) {
            return angular.lowercase(obj.hrmE_EmployeeFirstName).indexOf(angular.lowercase($scope.searchchkbx)) >= 0;
        };

        $scope.searchchkbx1 = "";
        $scope.filterchkbx1 = function (obj) {
            return angular.lowercase(obj.hrmE_EmployeeFirstName).indexOf(angular.lowercase($scope.searchchkbx)) >= 0;
        };

        $scope.isOptionsRequired3 = function () {
            return !$scope.emplist.some(function (options) {
                return options.checkedsub;
            });
        };

        $scope.isOptionsRequired4 = function () {
            return !$scope.emplistautjorizedby.some(function (options) {
                return options.checked;
            });
        };

          


        $scope.generateQRCode = function () {
            var requestData = {
                "VMAP_VisitorName": $scope.VMMV_VisitorName,
                "VMAP_VisitorEmailid": $scope.VMMV_VisitorEmailid  
            };

            apiService.create("AddVisitors/generateQRCode", requestData).then(function (promise) {
                $scope.image = promise.qrCodeImage;

                if (promise.returnVal === 'generated') {
                    swal("QR Code generated successfully");
                } else {
                    swal("Failed to generate QR Code");
                }
            }, function (error) {
                swal("Error", "Failed to generate QR Code", "error");
            });
        };


    }
})();