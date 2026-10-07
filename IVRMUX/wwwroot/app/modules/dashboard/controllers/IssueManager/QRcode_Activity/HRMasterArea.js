
(function () {
    'use strict';
    angular
        .module('app')
        .controller('HRMasterAreaController', HRMasterAreaController);
    HRMasterAreaController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache', '$compile','$filter'];
    function HRMasterAreaController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache, $compile, $filter) {


        $scope.editflag = false;

        $scope.searchValueU = "";
        $scope.searchValueI = "";
        $scope.searchValueT = "";
       
        var paginationformasters;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;

        }
        else {
            paginationformasters = 10;
        }

        $scope.usrname = localStorage.getItem('username');
        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        if (admfigsettings.length > 0) {
            var logopath = admfigsettings[0].asC_Logo_Path;
        }
        $scope.imgname = logopath;
        var configsettings = JSON.parse(localStorage.getItem("feeconfigsettings"));
        //-------------------------------------------------------------------


        //-----------------------------------------------------------------------
        $scope.obj.invmG_DepreciationApplFlg = true;
        $scope.obj.invmG_AppreciationApplFlg = false;
        $scope.obj.invmG_MaintenanceAplFlg = false;
        $scope.obj = {};
        $scope.ismeridian = true;
        $scope.hstep = 1;
        $scope.mstep = 1; 
        $scope.toggleMode = function () {
            $scope.ismeridian = !$scope.ismeridian;
        };
        $scope.loaddata = function () {
            $scope.currentPage = 1;
            $scope.itemsPerPage = paginationformasters;
            $scope.search = "";
            $scope.currentPageU = 1;
            $scope.itemsPerPageU = paginationformasters;

            $scope.currentPageI = 1;
            $scope.itemsPerPageI = paginationformasters;
            $scope.search = "";
            $scope.presentCountgridU = "";
            $scope.presentCountgridI = "";
            $scope.presentCountgridT = "";
            $scope.get_AreaActivityTime = [];
            var pageid = 2;
            apiService.getURI("HRMasterArea/getloaddata", pageid).
                then(function (promise) {

                    $scope.get_building = promise.get_building;
                    $scope.get_buildingmap = promise.get_building;

                    $scope.get_Area = promise.get_Area;
                    $scope.presentCountgridU = $scope.get_Area.length;

                    $scope.get_AreaActivity = promise.get_AreaActivity;
                    $scope.presentCountgridI = $scope.get_AreaActivity.length;

                    $scope.get_AreaActivityTimeTemp = [];
                    $scope.get_AreaActivityTimeTemp = promise.get_AreaActivityTime;
                    if ($scope.get_AreaActivityTimeTemp != null && $scope.get_AreaActivityTimeTemp.length > 0) {
                        for (var t = 0; t < $scope.get_AreaActivityTimeTemp.length; t++) {
                            $scope.get_AreaActivityTime.push({
                                hrmaractT_Time: moment($scope.get_AreaActivityTimeTemp[t].hrmaractT_Time, 'HH:mm a').format(),
                                hrmB_BuildingName: $scope.get_AreaActivityTimeTemp[t].hrmB_BuildingName,
                                hrmF_FloorName: $scope.get_AreaActivityTimeTemp[t].hrmF_FloorName,
                                hrmaR_AreaName: $scope.get_AreaActivityTimeTemp[t].hrmaR_AreaName,
                                hrmaractT_ActivityDuration: $scope.get_AreaActivityTimeTemp[t].hrmaractT_ActivityDuration,
                                hrmaracT_ActivityName: $scope.get_AreaActivityTimeTemp[t].hrmaracT_ActivityName,
                                hrmaractT_ActiveFlag: $scope.get_AreaActivityTimeTemp[t].hrmaractT_ActiveFlag,
                                hrmaractT_Id: $scope.get_AreaActivityTimeTemp[t].hrmaractT_Id,
                                hrmB_Id: $scope.get_AreaActivityTimeTemp[t].hrmB_Id,
                                hrmF_Id: $scope.get_AreaActivityTimeTemp[t].hrmF_Id,
                                hrmaR_Id: $scope.get_AreaActivityTimeTemp[t].hrmaR_Id,
                                hrmaracT_Id: $scope.get_AreaActivityTimeTemp[t].hrmaracT_Id,

                            });
                        }
                    }
                    $scope.presentCountgridT = $scope.get_AreaActivityTime.length;
                });
        };

        //-----------Floordeatils
        $scope.floordetailschange = function (hrmB_Id) {
            $scope.get_floor = [];
            $scope.get_flooramap = [];
            $scope.hrmF_Id = "";
            var data = {
                "HRMB_Id": hrmB_Id
            }
            apiService.create("HRMasterArea/getfloors", data).
                then(function (promise) {
                    if (promise.get_floor != null && promise.get_floor.length > 0) {
                        $scope.get_floor = promise.get_floor;
                        $scope.get_flooramap = promise.get_floor;
                    }
                    else {
                        swal("No Record Found....")
                    }

                })
        }

        //-----------Areadeatils
        $scope.areadetailschange = function (hrmB_Id) {
            $scope.get_getAreaName = [];
            var data = {
                "HRMB_Id": $scope.hrmmB_Id,
                "HRMF_Id": $scope.hrmmF_Id
            }
            apiService.create("HRMasterArea/getAreaName", data).
                then(function (promise) {
                    if (promise.get_getAreaName != null && promise.get_getAreaName.length > 0) {
                        $scope.get_getAreaName = promise.get_getAreaName;
                    }
                    else {
                        swal("No Record Found....")
                    }

                })
        }

        //-----------Areadeatils
        $scope.areadetailschangetab = function (hrmB_Id) {
            $scope.get_getAreaName = [];
            var data = {
                "HRMB_Id": $scope.hrmmmB_Id,
                "HRMF_Id": $scope.hrmmmF_Id
            }
            apiService.create("HRMasterArea/getAreaName", data).
                then(function (promise) {
                    if (promise.get_getAreaName != null && promise.get_getAreaName.length > 0) {
                        $scope.get_getAreaName = promise.get_getAreaName;
                    }
                    else {
                        swal("No Record Found....")
                    }

                })
        }


        //-----------Activity Deatils
        $scope.Activitydetailschange = function (hrmaR_Id) {
            $scope.get_getActivity = [];
            var data = {
                "HRMAR_Id": hrmaR_Id,
                
            }
            apiService.create("HRMasterArea/getActivityName", data).
                then(function (promise) {
                    if (promise.get_getActivity != null && promise.get_getActivity.length > 0) {
                        $scope.get_getActivity = promise.get_getActivity;
                    }
                    else {
                        swal("No Record Found....")
                    }

                })
        }
        
        $scope.generateQRCode = function () {
            var QRcodedetails = "";
            var Building = "";
            var Floor = "";
            var AreaName = "";
            var AreaCode = "";
            if ($scope.hrmaR_AreaCode != "") {
                for (var i = 0; i < $scope.get_building.length; i++) {
                    if ($scope.hrmB_Id == $scope.get_building[i].hrmB_Id) {
                        Building = $scope.get_building[i].hrmB_BuildingName;
                    }
                }
                for (var i = 0; i < $scope.get_floor.length; i++) {
                    if ($scope.hrmF_Id == $scope.get_floor[i].hrmF_Id) {
                        Floor = $scope.get_floor[i].hrmF_FloorName;
                    }
                }
                AreaName = $scope.hrmaR_AreaName;
                AreaCode = $scope.hrmaR_AreaCode;
            }
            var data = {
                "HRMB_Id": $scope.hrmB_Id,
                "HRMF_Id": $scope.hrmF_Id,
                "HRMAR_AreaName": AreaName,
                "HRMAR_AreaCode": AreaCode 

            };
            apiService.create("HRMasterArea/FloorQRCODE", data).then(function (promise) {
               

            });
            //QRcodedetails = Building.replace(/\s/g, "") + '/' + Floor.replace(/\s/g, "") + '/' + AreaName.replace(/\s/g, "") + '/' + AreaCode.replace(/\s/g, "");
            //$scope.obj.QRCode = QRcodedetails;
        }
        $scope.QRdetailschange = function (QRcodedetails) {
            var variable = $scope.qrinputvalues;
            let website = document.getElementById("website").value;
            if (website) {
                let qrcodeContainer = document.getElementById("qrcode");
                qrcodeContainer.innerHTML = "";
                new QRCode(qrcodeContainer, website);
                document.getElementById("qrcode-container").style.display = "block";
            } else {
                alert("Please enter a valid URL");
            }
        }
        
        //---------------------------------Save--------------------------------------------
        

        //Master Area
        $scope.savedataAREA = function () {
            $scope.submittedUG = true;
            if ($scope.myForm1.$valid) {
                var QRcodedetails = "";
                var Building = "";
                var Floor = "";
                var AreaName = "";
                var AreaCode = "";
                if ($scope.hrmaR_AreaCode != "") {
                    for (var i = 0; i < $scope.get_building.length; i++) {
                        if ($scope.hrmB_Id == $scope.get_building[i].hrmB_Id) {
                            Building = $scope.get_building[i].hrmB_BuildingName;
                        }
                    }
                    for (var i = 0; i < $scope.get_floor.length; i++) {
                        if ($scope.hrmF_Id == $scope.get_floor[i].hrmF_Id) {
                            Floor = $scope.get_floor[i].hrmF_FloorName;
                        }
                    }
                    AreaName = $scope.hrmaR_AreaName;
                    AreaCode = $scope.hrmaR_AreaCode;
                }
                var data = {
                         "HRMB_BuildingName": Building,
                         "HRMF_FloorName": Floor,
                        "HRMAR_AreaName": AreaName,
                        "HRMAR_Id": $scope.hrmaR_Id,
                        "HRMF_Id": $scope.hrmF_Id,
                        "HRMB_Id": $scope.hrmB_Id,
                        "HRMAR_AreaName": $scope.hrmaR_AreaName,
                        "HRMAR_AreaCode": $scope.hrmaR_AreaCode,
                        //"HRMA_QRCODEPath":$scope.obj.QRCode,
                        //"HRMAR_TypeFlag": "Area",
                        //"HRMAR_QRCode": sigText.innerHTML = imagarr[1]
                    }
                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                }
                apiService.create("HRMasterArea/savedetailsAREA", data).then(function (promise) {

                    $scope.hrmaR_QRCode = promise.hrmaR_QRCode

                    if (promise.returnduplicatestatus == 'Saved') {
                        swal('Record Saved Successfuly');
                        $state.reload();
                    }
                    else if (promise.returnduplicatestatus == 'Not Saved') {
                        swal('Record Not Saved Successfuly');
                    }
                    else if (promise.returnduplicatestatus == 'Update') {
                        swal('Record Update Successfuly');
                        $state.reload();
                    }
                    else if (promise.returnduplicatestatus == 'Not Update') {
                        swal('Record Not Update Successfuly');
                    }
                    else if (promise.returnduplicatestatus == 'Duplicate') {
                        swal('Record already exist');
                    }
                    else {
                        if (promise.hrmaR_Id == 0 || promise.hrmaR_Id < 0) {
                            swal('Failed to save, please contact administrator');
                        }
                        else if (promise.hrmaR_Id > 0) {
                            swal('Failed to update, please contact administrator');
                        }
                    }
                   
                    $scope.submittedUG = false;
                })
            }
            else {
                $scope.submittedUG = true;
            }
        };


        $scope.edit = function (bil) {
            $scope.editflag = true;
            $scope.hrmaR_Id = bil.hrmaR_Id;
            var data = {
                "HRMAR_Id": $scope.hrmaR_Id
            };
            apiService.create("HRMasterArea/edit", data).then(function (promise) {
                $scope.Edit_Data = promise.edit_Data;
                $scope.hrmaR_Id = promise.edit_Data[0].hrmaR_Id;
                $scope.hrmaR_AreaCode = promise.edit_Data[0].hrmaR_AreaCode;
                $scope.hrmaR_AreaName = promise.edit_Data[0].hrmaR_AreaName;
                $scope.hrmB_BuildingName = promise.edit_Data[0].hrmB_BuildingName;
                $scope.hrmF_FloorName = promise.edit_Data[0].hrmF_FloorName;
                $scope.hrmB_Id = promise.edit_Data[0].hrmB_Id;
               var abc = $scope.hrmB_Id
                $scope.floordetailschange(abc);
                $scope.hrmF_Id = promise.edit_Data[0].hrmF_Id;
             



            })

        };
        
        $scope.viewQR = function (promise) {
            $('#QRCodeview').modal('show'); 

            //var e1 = angular.element(document.getElementById("test"));
            //$compile(e1.html(promise.hrmaR_QRCode))(($scope));
            $scope.hrmaR_QRCode=promise.hrmaR_QRCode

            $scope.hrmB_BuildingNameaa = promise.hrmB_BuildingName;
            $scope.hrmF_FloorNameaa = promise.hrmF_FloorName;
            $scope.hrmaR_AreaNameaa = promise.hrmaR_AreaName;
            $scope.hrmaR_AreaCodeaa = promise.hrmaR_AreaCode;
        };
        
        //$scope.editU = function (item) {
        //    $scope.hrmB_Id = item.hrmB_Id;
        //    $scope.floordetailschange($scope.hrmB_Id);
        //    $scope.hrmF_Id = item.hrmF_Id;
        //    $scope.hrmaR_AreaName = item.hrmaR_AreaName;
        //    $scope.hrmaR_AreaCode = item.hrmaR_AreaCode;
        //    $scope.obj.QRCode = item.hrmaR_QRCode;
        //    $scope.editflag = true;
        //    $scope.hrmaR_Id = item.hrmaR_Id;
        //};


        $scope.deactive = function (item, SweetAlert) {
            $scope.hrmaR_ActiveFlag = item.hrmaR_ActiveFlag;
            var dystring = "";
            if (item.hrmaR_ActiveFlag == true) {
                dystring = "Deactivate";
            }
            else if (item.hrmaR_ActiveFlag == false) {
                dystring = "Activate";
            }
            swal({
                title: "Are you sure?",
                text: "Do You Want To " + dystring + " Record?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, " + dystring + " it",
                cancelButtonText: "Cancel",
                closeOnConfirm: false,
                closeOnCancel: false
            },
                function (isConfirm) {
                    if (isConfirm) {
                        apiService.create("HRMasterArea/deactiveArea", item).
                            then(function (promise) {
                                if (promise.returnval == true) {
                                    swal("Record " + dystring + "d Successfully!!!");
                                }
                                else {
                                    swal("Record Not " + dystring + "d Successfully!!!");
                                }
                                $state.reload();
                            })
                    }
                    else {
                        swal("Record " + dystring + "d Cancelled!!!");
                    }
                });
        }




         //Master Activity


        $scope.savedataAREAActivity = function () {
            $scope.submittedUGActivity = true;
            if ($scope.myForm2.$valid) {
                var data = {
                    "HRMARACT_Id": $scope.hrmaracT_Idupdate,
                    "HRMAR_Id": $scope.hrmaR_Id,
                    "HRMARACT_ActivityName" : $scope.hrmaracT_ActivityName,
                    "HRMARACT_ActivityDesc": $scope.hrmaracT_ActivityDesc,
                  
                }
                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                }
                apiService.create("HRMasterArea/savedetailsAREAActivity", data).then(function (promise) {

                    if (promise.returnduplicatestatus == 'Saved') {
                        swal('Record Saved Successfuly');
                        $state.reload();
                    }
                    else if (promise.returnduplicatestatus == 'Not Saved') {
                        swal('Record Not Saved Successfuly');
                    }
                    else if (promise.returnduplicatestatus == 'Update') {
                        swal('Record Update Successfuly');
                        $state.reload();
                    }
                    else if (promise.returnduplicatestatus == 'Not Update') {
                        swal('Record Not Update Successfuly');
                    }
                    else if (promise.returnduplicatestatus == 'Duplicate') {
                        swal('Record already exist');
                    }
                    else {
                        if (promise.hrmaracT_Id == 0 || promise.hrmaracT_Id < 0) {
                            swal('Failed to save, please contact administrator');
                        }
                        else if (promise.hrmaracT_Id > 0) {
                            swal('Failed to update, please contact administrator');
                        }
                    }

                    $scope.submittedUGActivity = false;
                })
            }
            else {
                $scope.submittedUGActivity = true;
            }
        };
        $scope.editAN = function (item) {
            $scope.hrmaracT_ActivityName = item.hrmaracT_ActivityName;
            $scope.hrmaracT_ActivityDesc = item.hrmaracT_ActivityDesc;
            $scope.hrmmB_Id = item.hrmB_Id;
            $scope.floordetailschange(item.hrmB_Id);
            $scope.hrmmF_Id = item.hrmF_Id;
            $scope.areadetailschange(item.hrmF_Id);
            $scope.hrmaR_Id = item.hrmaR_Id;
            $scope.hrmaracT_Idupdate = item.hrmaracT_Id;
            
        };
 
        $scope.deactiveA = function (item, SweetAlert) {
            $scope.hrmaracT_ActiveFlag = item.hrmaracT_ActiveFlag;
            var dystring = "";
            if (item.hrmaracT_ActiveFlag == true) {
                dystring = "Deactivate";
            }
            else if (item.hrmaracT_ActiveFlag == false) {
                dystring = "Activate";
            }
            swal({
                title: "Are you sure?",
                text: "Do You Want To " + dystring + " Record?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, " + dystring + " it",
                cancelButtonText: "Cancel",
                closeOnConfirm: false,
                closeOnCancel: false
            },
                function (isConfirm) {
                    if (isConfirm) {
                        apiService.create("HRMasterArea/deactiveAreaActivity", item).
                            then(function (promise) {
                                if (promise.returnval == true) {
                                    swal("Record " + dystring + "d Successfully!!!");
                                }
                                else {
                                    swal("Record Not " + dystring + "d Successfully!!!");
                                }
                                $state.reload();
                            })
                    }
                    else {
                        swal("Record " + dystring + "d Cancelled!!!");
                    }
                });
        }
        
       //Master  Activity_Time 
        $scope.savedataAREAActivityTime = function () {
            $scope.submittedUGActivityTime = true;
            if ($scope.myForm3.$valid) {
                var VTADAA_DepartureTime = $scope.hrmaractT_Time;

                $scope.sresult = $filter('date')(VTADAA_DepartureTime, 'HH:mm');
                var data = {
                    
                    "HRMARACTT_Id": $scope.hrmaractT_Idupdate,
                    "HRMARACT_Id": $scope.hrmaracT_Id,
                    "HRMARACTT_Time": $scope.sresult,
                    "HRMARACTT_ActivityDuration": $scope.hrmaractT_ActivityDuration,

                }
                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                }
                apiService.create("HRMasterArea/savedetailsAREAActivityTime", data).then(function (promise) {

                    if (promise.returnduplicatestatus == 'Saved') {
                        swal('Record Saved Successfuly');
                        $state.reload();
                    }
                    else if (promise.returnduplicatestatus == 'Not Saved') {
                        swal('Record Not Saved Successfuly');
                    }
                    else if (promise.returnduplicatestatus == 'Update') {
                        swal('Record Update Successfuly');
                        $state.reload();
                    }
                    else if (promise.returnduplicatestatus == 'Not Update') {
                        swal('Record Not Update Successfuly');
                    }
                    else if (promise.returnduplicatestatus == 'Duplicate') {
                        swal('Record already exist');
                    }
                    else {
                        if (promise.hrmaractT_Id == 0 || promise.hrmaractT_Id < 0) {
                            swal('Failed to save, please contact administrator');
                        }
                        else if (promise.hrmaractT_Id > 0) {
                            swal('Failed to update, please contact administrator');
                        }
                    }

                    $scope.submittedUGActivityTime = false;
                })
            }
            else {
                $scope.submittedUGActivityTime = true;
            }
        };


        $scope.editT = function (item) {
            $scope.hrmaractT_Time = item.hrmaractT_Time;
            $scope.hrmaractT_ActivityDuration = item.hrmaractT_ActivityDuration;
            $scope.hrmmmB_Id = item.hrmB_Id;
            $scope.floordetailschange($scope.hrmmmB_Id);
            $scope.hrmmmF_Id = item.hrmF_Id;
            $scope.areadetailschangetab(item.hrmF_Id);
            $scope.hrmmaR_Id = item.hrmaR_Id;
            $scope.Activitydetailschange(item.hrmaR_Id);
            $scope.hrmaracT_Id = item.hrmaracT_Id;
            $scope.hrmaractT_Idupdate = item.hrmaractT_Id;
          
        };



        $scope.deactiveT = function (item, SweetAlert) {
            $scope.hrmaractT_ActiveFlag = item.hrmaractT_ActiveFlag;
            var dystring = "";
            if (item.hrmaractT_ActiveFlag == true) {
                dystring = "Deactivate";
            }
            else if (item.hrmaractT_ActiveFlag == false) {
                dystring = "Activate";
            }
            swal({
                title: "Are you sure?",
                text: "Do You Want To " + dystring + " Record?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, " + dystring + " it",
                cancelButtonText: "Cancel",
                closeOnConfirm: false,
                closeOnCancel: false
            },
                function (isConfirm) {
                    if (isConfirm) {
                        apiService.create("HRMasterArea/deactiveAreaActivityTime", item).
                            then(function (promise) {
                                if (promise.returnval == true) {
                                    swal("Record " + dystring + "d Successfully!!!");
                                }
                                else {
                                    swal("Record Not " + dystring + "d Successfully!!!");
                                }
                                $state.reload();
                            })
                    }
                    else {
                        swal("Record " + dystring + "d Cancelled!!!");
                    }
                });
        }

       
        
        $scope.interacted1 = function (field) {
            return $scope.submittedUG;
        };
        $scope.interacted2 = function (field) {
            return $scope.submittedUGActivity;
        };
        $scope.interacted3 = function (field) {
            return $scope.submittedUGActivityTime;
        };
        
        $scope.sortBy = function (propertyName) {
            $scope.reverse = ($scope.propertyName === propertyName) ? !$scope.reverse : false;
            $scope.propertyName = propertyName;
        };
        
        $scope.searchValueI = '';
        $scope.searchValueU = '';
        $scope.searchValueT = '';

        $scope.cancelU = function () {
            $state.reload();
        }
        $scope.cancelI = function () {
            $state.reload();
        }
        $scope.cancelT = function () {
            $state.reload();
        }



        $scope.printData = function () {
            var innerContents = "";
            innerContents = document.getElementById("printDeviation").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
            popupWinindow.document.close();
        };


        
        
    }
})();