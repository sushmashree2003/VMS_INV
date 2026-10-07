(function () {
    'use strict';
    angular
        .module('app')
        .controller('VMS_CandidateMeeting_StartController', VMS_CandidateMeeting_StartController);

    VMS_CandidateMeeting_StartController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$stateParams', '$filter', 'superCache'];
    function VMS_CandidateMeeting_StartController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $stateParams, $filter, superCache) {
        //object

        $scope.ttt = true;
        $scope.hostname = "";
        $scope.roomName = "";
        $scope.meetid = 0;
        $scope.showstaff = true;
        $scope.clearallsettings = function () {
            $state.reload();
        };

        //Employeee
        $scope.currentPage = 1;
        $scope.currentPage3 = 1;
        $scope.itemsPerPage = 5;
        $scope.currentPage1 = 1;
        $scope.itemsPerPage1 = 5;
        $scope.currentPage2 = 1;
        $scope.itemsPerPage2 = 10;
        $scope.itemsPerPage3 = 10;
        $scope.page1 = "page1";
        $scope.page2 = "page2";
        $scope.page3 = "page3";
        $scope.page4 = "page4";
        $scope.LMSLMEET_PlannedDate = new (Date);
        $scope.sort = function (keyname) {
            $scope.sortKey = keyname;   //set the sortKey to the param passed
            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
        }

        // Get form Details at onload 
        $scope.onLoadGetData = function () {
            var pageid = 2;
            apiService.getDATA("VMS_Live_Interview/getempdetails").then(function (promise) {
                $scope.meetinglist = promise.meetinglist;
                $scope.vcflag = promise.vcflag;
                if (promise.meetingstart != null || promise.meetingstart > 0) {
                    $scope.meetingstart = promise.meetingstart;
                }
            });
        };


       
        $scope.type = '';
        $scope.meetingst = false;
        $scope.onstartmeeting = function (user) {
            $scope.topic = user.VMSLMEET_MeetingTopic;
            $scope.meetingst = true;
            $scope.meetid = user.VMSLMEET_MeetingId;
           
            var data = {
                "VMSLITVW_Id": user.VMSLITVW_Id,
                "VMSLMEET_MeetingId": user.VMSLMEET_MeetingId,
                "VMSLMEET_MeetingTopic": user.VMSLMEET_MeetingTopic,
                "HRME_Id": user.HRME_Id,
            }
            apiService.create("VMS_Live_Interview/onstartmeeting", data).then(function (promise) {
                if ($scope.vcflag === "BBB") {
                    $scope.murl = promise.moderatorurl;
                    window.open($scope.murl, "_self");
                };
            });
        };

        $scope.stopstreeming = function (user) {
           
            var data = {
                "VMSLITVW_Id": user,
              
            };

            $scope.ttt = false;
            apiService.create("VMS_Live_Interview/endmainmeeting", data).then(function (promise) {
                //$scope.meetinglist = promise.meetinglist;
                if (promise.returnval == true) {

                    swal('Meeting Completed')
                    $state.reload();
                }

            });
        }


        $scope.topic = "";
        $scope.joinmeeting = function (user) {
            debugger;
            $scope.ttt = false;
            $scope.meetingst = true;
            $scope.type = 'ST';
            $scope.topic = user.lmslmeeT_MeetingTopic;
            $scope.meetid = user.lmslmeeT_Id
            apiService.create("VMS_Live_Meeting/joinmeeting", user).then(function (promise) {
                //$scope.meetinglist = promise.meetinglist;
            });
        };

        $scope.ondatechange = function () {
            var fromdate1 = $scope.LMSLMEET_PlannedDate === null ? "" : $filter('date')($scope.LMSLMEET_PlannedDate, "yyyy-MM-dd");
            var data = {
                "LMSLMEET_PlannedDate": fromdate1
            };
            apiService.create("VMS_Live_Interview/ondatechange", data).then(function (promise) {
                $scope.meetinglist = promise.meetinglist;
                $scope.joinmeetinglist = promise.joinmeetinglist;
            });
        };

        // retun details
        $scope.employeeDetails = [];
        $scope.searchValue = '';


        $scope.currentemployeequalificationDetails = [];
        $scope.employequalify = false;
        $scope.employeedocumentflg = false;
        $scope.employeeclasssubjectflg = false;
        $scope.qualifymsg = false;
        $scope.empdetails = {};
        $scope.empqualifydetails = {};
        //Search employee
        $scope.submitted = false;

      

        $scope.OnEmployeeChange = function () {
            $scope.qualifymsg = false;
        };

        $scope.all_check_empl = function (empl) {
            var toggleStatus4 = empl;
            angular.forEach($scope.employeedropdown, function (itm) {
                itm.emple = toggleStatus4;
            });
        };

        $scope.searchDiv = true;
        $scope.empDiv = false;


        //Clear data
        $scope.Employee = {};
        $scope.cleardata = function () {
            $scope.Employee = {};
            $scope.employeeDetails = [];
            $scope.employequalification = [];
            $scope.employeedocument = [];
            $scope.empdetails = {};
            $scope.searchDiv = true;
            $scope.empDiv = false;
            $scope.DesignationName = "";
            $scope.groupTypeselectedAll = false;
            $scope.departmentselectedAll = false;
            $scope.designationselectedAll = false;
            $scope.submitted = false;
            $scope.employequalify = false;
            $scope.qualifymsg = false;
            $scope.designationselectedAll = false;

            $scope.UploadEmployeeProfilePic = [];
            $('#blah').removeAttr('src');
            $scope.search = "";

            $scope.myForm.$setPristine();
            $scope.myForm.$setUntouched();
            $scope.onLoadGetData();

        };

        $scope.interacted = function (field) {
            return $scope.submitted;
        };


        //By group Type
        //By group Type
        $scope.GetEmployeeBygroupTypeAll = function (groupTypeselectedAll) {
            if ($scope.EmployeeDis) {
                $scope.EmployeeDis = false;
            }
            var toggleStatus = $scope.groupTypeselectedAll;

            angular.forEach($scope.groupTypedropdown, function (itm) {
                itm.selected = toggleStatus;
            });

            angular.forEach($scope.designationdropdown, function (itm22) {
                itm22.selected = toggleStatus;
                $scope.designationselectedAll = toggleStatus;
            });

            angular.forEach($scope.departmentdropdown, function (itm232) {
                itm232.selected = toggleStatus;
                $scope.departmentselectedAll = toggleStatus;
            });

            $scope.get_depts();
        };



        //single
        $scope.GetEmployeeBygroupType = function (groupType) {
            if ($scope.EmployeeDis) {
                $scope.EmployeeDis = false;
            }
            $scope.groupTypeselectedAll = $scope.groupTypedropdown.every(function (itm) {

                return itm.selected;
            });

            $scope.get_depts();
        };

    






     


        //By Department Single
        $scope.GetEmployeeByDepartment = function (department) {
            if ($scope.EmployeeDis) {
                $scope.EmployeeDis = false;
            }
            $scope.departmentselectedAll = $scope.departmentdropdown.every(function (itm) {

                return itm.selected;
            });
            $scope.get_desig();
        };

        $scope.get_desig = function () {
            var ids = [];
            angular.forEach($scope.groupTypedropdown, function (grp_t) {
                if (grp_t.selected) {
                    ids.push(grp_t.hrmgT_Id);
                }
            });
            var ids1 = [];
            angular.forEach($scope.departmentdropdown, function (grp_t) {
                if (grp_t.selected) {
                    ids1.push(grp_t.hrmD_Id);
                }
            });
            var data = {
                hrmgT_IdList: ids,
                hrmD_IdList: ids1
            };
            apiService.create("LiveMeetingSchedule/get_desig", data).
                then(function (promise) {
                    if (promise.designationdropdown !== null && promise.designationdropdown.length > 0) {
                        $scope.designationdropdown = promise.designationdropdown;
                        $scope.designationselectedAll = true;
                        $scope.GetEmployeeByDesignationAll($scope.designationselectedAll);
                    }
                });
        };


        //By Designation
        $scope.GetEmployeeByDesignationAll = function (designationselectedAll) {
            if ($scope.EmployeeDis) {
                $scope.EmployeeDis = false;
            }
            var toggleStatus = $scope.designationselectedAll;
            angular.forEach($scope.designationdropdown, function (itm) {
                itm.selected = toggleStatus;

            });

        };


        //By Designation Single
        $scope.GetEmployeeByDesignation = function (designation) {
            if ($scope.EmployeeDis) {
                $scope.EmployeeDis = false;
            }
            $scope.designationselectedAll = $scope.designationdropdown.every(function (itm) {

                return itm.selected;
            });
        };




        $scope.SearchEmployeeDetails = function () {

            var groupTypeselected = [];

            angular.forEach($scope.groupTypedropdown, function (itm) {
                if (itm.selected) {
                    groupTypeselected.push(itm.hrmgT_Id);//
                }
            });

            var employeeTypeselected = [];
            angular.forEach($scope.employeeTypedropdown, function (itm) {
                if (itm.selected) {
                    employeeTypeselected.push(itm.hrmeT_Id);
                }

            });

            var departmentselected = [];
            angular.forEach($scope.departmentdropdown, function (itm) {
                if (itm.selected) {
                    departmentselected.push(itm.hrmD_Id);
                }

            });


            var designationselected = [];
            angular.forEach($scope.designationdropdown, function (itm) {
                if (itm.selected) {
                    designationselected.push(itm.hrmdeS_Id);
                }

            });

            if (groupTypeselected.length === 0 && departmentselected.length === 0 && designationselected.length === 0) {
                swal('Kindly select atleast one record');
                return;
            }
            $scope.employeedropdown = [];
            var data = {
                hrmgT_IdList: groupTypeselected,
                hrmD_IdList: departmentselected,
                hrmdeS_IdList: designationselected

            };

            apiService.create("LiveMeetingSchedule/filterEmployeedetailsBySelection", data).
                then(function (promise) {

                    if (promise.employeedropdown !== null && promise.employeedropdown.length > 0) {
                        $scope.employeedropdown = promise.employeedropdown;
                        $scope.searchDiv = false;
                        $scope.empDiv = true;
                    }
                    else {
                        swal('No Record found to display .. !');
                        return;
                    }

                });
        };






        //$scope.printData = function () {
        //    var divToPrint = document.getElementById("Table");
        //        var newWin = window.open();
        //        newWin.document.write(divToPrint.outerHTML);
        //        newWin.print();
        //        newWin.close();
        //        // $state.reload();
        //    }

        $scope.printToCart = function (Baldwin) {
            var innerContents = document.getElementById("Baldwin").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/pfchallan/PFChallanPdf.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
                '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
            popupWinindow.document.close();
        };

        $scope.showmodaldetails = function (data) {
            $('#preview').removeAttr('src');
            var filename = data.hrmedS_DocumentImageName.toString();
            var nameArray = filename.split('.');
            var extention = nameArray[nameArray.length - 1];

            if (extention === "jpg" || extention === "jpeg") {
                $('#preview').attr('src', data.hrmedS_DocumentImageName);
            }
            else if (extention === "doc" || extention === "docx") {
                $('#preview').removeAttr('src');
            }
            else if (extention === "pdf") {
                $scope.content = 'https://s3-us-west-2.amazonaws.com/s.cdpn.io/149125/relativity.pdf';
            }
        };

        $scope.zoomin = function () {
            var myImg = document.getElementById("preview");
            var currWidth = myImg.clientWidth;
            if (currWidth >= 750) {
                swal("Maximum zoom-in level reached.");
            } else {
                myImg.style.width = (currWidth + 50) + "px";
            }
        };

        $scope.zoomout = function () {
            var myImg = document.getElementById("preview");
            var currWidth = myImg.clientWidth;
            if (currWidth <= 400) {
                swal("Maximum zoom-out level reached.");
            } else {
                myImg.style.width = (currWidth - 50) + "px";
            }
        };

    }


})();