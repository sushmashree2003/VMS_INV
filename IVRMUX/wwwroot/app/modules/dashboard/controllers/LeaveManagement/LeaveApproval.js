
(function () {
    'use strict';
    angular.module('app').controller('LeaveApprovalController', LeaveApprovalController)

    LeaveApprovalController.$inject = ['$rootScope', '$scope', '$state', '$location', 'apiService', 'Flash', '$http', '$q', '$stateParams', '$window', 'superCache', '$filter', 'uiGridConstants', 'appSettings', '$sce']
    function LeaveApprovalController($rootScope, $scope, $state, $location, apiService, Flash, $http, $q, $stateParams, $window, superCache, $filter, $uiGridConstants, appSettings, $sce) {

        var copty = "";
        $scope.rowsSelected = [];

        //$scope.itemsPerPage = 10;
        //$scope.itemsPerPage2 = 5;
        //if ($scope.itemsPerPage == undefined || $scope.itemsPerPage == null) {
        //    $scope.itemsPerPage = 5;
        //}


        var paginationformasters;
        var copty;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings != null && ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
            copty = ivrmcofigsettings[0].ivrmgC_Disclaimer;
        } else {
            paginationformasters = 10;
        }

        $scope.itemsPerPage = paginationformasters;

        $scope.currentPage = 1;
        $scope.currentPage2 = 1;
        $scope.coptyright = copty;

        $scope.searchValue = '';
        $scope.filterValue = function (obj) {
            return (angular.lowercase(obj.hrmE_EmployeeFirstName)).indexOf(angular.lowercase($scope.searchValue)) >= 0 ||
                (angular.lowercase(obj.hrmL_LeaveName)).indexOf(angular.lowercase($scope.searchValue)) >= 0 ||
                (angular.lowercase(obj.hrelT_Status)).indexOf(angular.lowercase($scope.searchValue)) >= 0
        };
        $scope.get_leavestatus = [];
        $scope.leavebalance = [];
        $scope.loadData = function () {
            var id = 2;

            apiService.getURI("LeaveApproval/getApprovalStatus", id).then(function (promise) {

                $scope.get_leavestatus = promise.get_leavestatus;
                $scope.employeeleavedetails = promise.employeeleavedetails;
                $scope.trustAsHtml = function (html) {
                    return $sce.trustAsHtml(html);
                };

                angular.forEach($scope.get_leavestatus, function (grdobj) {
                    grdobj.HRELAP_FromDate = new Date(grdobj.HRELAP_FromDate);
                    grdobj.HRELAP_ToDate = new Date(grdobj.HRELAP_ToDate);
                    grdobj.HRELAP_TotalDaysapprv = grdobj.HRELAP_TotalDays;
                    grdobj.mindate = new Date(grdobj.HRELAP_FromDate);
                    grdobj.maxdate = new Date(grdobj.HRELAP_ToDate);
                });

                //angular.forEach($scope.employeeleavedetails, function (row1) {
                //    angular.forEach($scope.leavestatus, function (row) {
                //        if (row.HRME_Id == row1.HRME_Id) {
                //            $scope.get_leavestatus.push({
                //                HRME_EmployeeFirstName: row.HRME_EmployeeFirstName,
                //                HRELAP_SupportingDocument: row.HRELAP_SupportingDocument,
                //                HRML_LeaveName: row.HRML_LeaveName,
                //                HRELAP_ApplicationDate: row.HRELAP_ApplicationDate,
                //                HRELAP_TotalDays: row.HRELAP_TotalDays,
                //                HRELAP_ReportingDate: row.HRELAP_ReportingDate,
                //                HRELAP_FromDate: row.HRELAP_FromDate,
                //                HRELAP_ToDate: row.HRELAP_ToDate,
                //                HRELAP_TotalDaysapprv: row.HRELAP_TotalDays,
                //                mindate: row.HRELAP_FromDate,
                //                maxdate: row.HRELAP_ToDate,                                                              
                //            })
                //            $scope.leavebalance.push({  
                //                LeaveType: row1.LeaveType,
                //                EmpBalanceLeaves: row1.EmpBalanceLeaves,
                //            })
                //        }
                //    })
                //});

               

                $scope.temp_leave = promise.get_leavestatus;
                $scope.activityIds = promise.activityIds;
                if ($scope.count === 0) {
                    swal("Data not Found !!");
                    $scope.ckdept = false;
                }

                //$scope.fromminDate = new Date(
                //    $scope.setmindate.getFullYear(),
                //    $scope.setmindate.getMonth() - 12,
                //    $scope.setmindate.getDate());

                //$scope.tominDate = new Date(
                //    $scope.fromDate.getFullYear(),
                //    $scope.fromDate.getMonth() - 12,
                //    $scope.fromDate.getDate());

                //$scope.minDate = new Date(
                //    $scope.toDate.getFullYear(),
                //    $scope.toDate.getMonth(),
                //    $scope.toDate.getDate() + 1);
                //$scope.maxDate = new Date(
                //    $scope.toDate.getFullYear(),
                //    $scope.toDate.getMonth(),
                //    $scope.toDate.getDate() + 1);

                $scope.HRELAP_ReportingDate = $scope.minDate;
            });
        };



        $scope.redirectpage = function () {
            $state.go('app.compoffODApproval');
        };



        $scope.Hlfday = function (row) {
            debugger;
            //var rowsss = $scope.gridApi.selection.getSelectedRows();
            var tempdays = row.HRELAP_TotalDaysapprv;
            if (row.selected == true) {
                if (row.cklop == true) {
                    row.HRELAP_TotalDaysapprv = tempdays - 0.5;
                }
                else if (row.cklop == false) {
                    row.HRELAP_TotalDaysapprv = tempdays + 0.5;
                }
            }
        };


        $scope.get_status = function (leavestatus) {
            var temp_array = [];
            $scope.approveorreject = "";
            if ($scope.myForm.$valid) {
                var statusswal = "";
                if (leavestatus=="Approved") {
                    statusswal = "Approve";
                }
                else {
                    statusswal = "Reject";
                }


                swal({
                    title: "Are you sure",
                    text: "Do you want to"+ statusswal+ "Leave?",
                    type: "warning",
                    showCancelButton: true,
                    confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, " + statusswal + " it!",
                    cancelButtonText: "Cancel",
                    closeOnConfirm: false,
                    closeOnCancel: false
                },
                    function (isConfirm) {
                        if (isConfirm) {
                            angular.forEach($scope.get_leavestatus, function (row) {
                                if (row.selected == true) {
                                    temp_array.push({
                                        HRELAP_ApplicationID: row.HRELAP_Id,
                                        HRELAP_LeaveReason: row.HRELAP_LeaveReason,
                                        HRELAPA_Remarks: row.HRELAPA_Remarks,
                                        HRME_Id: row.HRME_Id,
                                        HRML_Id: row.HRML_Id,
                                        HRELAP_TotalDays: row.HRELAP_TotalDays,
                                        HRELAP_TotalDaysapprv: row.HRELAP_TotalDaysapprv,
                                        HRELAP_FromDate: new Date(row.mindate).toDateString(),
                                        HRELAP_ToDate: new Date(row.maxdate).toDateString(),
                                        HRELAP_FromDateapprv: new Date(row.HRELAP_FromDate).toDateString(),
                                        HRELAP_ToDateapprv: new Date(row.HRELAP_ToDate).toDateString()
                                    });
                                }
                            });

                            var data = {
                                get_leave_status: temp_array,
                                "HRELAPA_Remarks": $scope.remarkstxta,
                                "status": leavestatus
                            };

                            apiService.create("LeaveApproval/get_status", data).then(function (promise) {

                                if (promise.message == "Success") {
                                    swal("Leave" + leavestatus+" Successfully");
                                    $state.reload();
                                }
                                else if (promise.message == "Balance Crossed") {
                                    swal("No sufficient Leave Balance for Approval. Kindly Reject The Leave.");
                                    $state.reload();
                                }
                                else if (promise.message == "Leave Is Not Credited") {
                                    swal("Leave Is Not Credited For Applied Year, Kindly Reject The Leave");
                                    $state.reload();
                                }
                                else {
                                    swal("Something went wrong!!");
                                    $state.reload();
                                }
                            });
                        }
                        else {
                            swal("Leave approval Cancelled");
                            $state.reload();
                        }
                    });
            }
            else {
                $scope.approveorreject = "Approve";
                $scope.submitted = true;
            }
        };

        $scope.reject_status = function () {
            var temp_array = [];
            $scope.approveorreject = "";
            if ($scope.myForm.$valid) {

                swal({
                    title: "Are you sure",
                    text: "Do you want to Reject Leave?",
                    type: "warning",
                    showCancelButton: true,
                    confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, Reject it!",
                    cancelButtonText: "Cancel",
                    closeOnConfirm: false,
                    closeOnCancel: false
                },
                    function (isConfirm) {
                        if (isConfirm) {
                            angular.forEach($scope.get_leavestatus, function (row) {
                                if (row.selected == true) {
                                    temp_array.push({
                                        HRELAP_ApplicationID: row.HRELAP_ApplicationID,
                                        HRELAP_LeaveReason: row.HRELAP_LeaveReason,
                                        HRME_Id: row.HRME_Id,
                                        HRML_Id: row.HRML_Id,
                                        HRELAP_TotalDays: row.HRELAP_TotalDays,
                                        HRELAP_TotalDaysapprv: row.HRELAP_TotalDaysapprv,
                                        HRELAPA_Remarks: row.HRELAPA_Remarks,
                                        HRELAP_FromDate: new Date(row.mindate).toDateString(),
                                        HRELAP_ToDate: new Date(row.maxdate).toDateString(),
                                        HRELAP_FromDateapprv: new Date(row.HRELAP_FromDate).toDateString(),
                                        HRELAP_ToDateapprv: new Date(row.HRELAP_ToDate).toDateString()
                                    });
                                }
                            });

                            var data = {
                                get_leave_status: temp_array,
                                "HRELAPA_Remarks": $scope.remarkstxta
                            };

                            apiService.create("LeaveApproval/reject_status", data).then(function (promise) {
                                if (promise.message == "Success") {
                                    swal("Rejected...");
                                    $state.reload();
                                }
                                else {
                                    swal("Something went wrong!!");
                                }
                            });

                        }
                        else {
                            swal("Leave Rejection Cancelled");
                        }
                    });
            }
            else {
                $scope.approveorreject = "Reject";
                $scope.submitted = true;
            }
        };

        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        $scope.fromdate = function (d1, row) {
            $scope.From_Date = d1;
            angular.forEach($scope.gridleavestatus.data, function (grdobj) {
                if (grdobj.hrelaP_Id === row.entity.hrelaP_Id) {
                    grdobj.toDate = d1;
                }
                if (grdobj.hrelS_CBLeaves !== 0) {
                    grdobj.hrelaP_TotalDays = 1;
                }
            });

            $scope.tominDate = new Date(
                $scope.From_Date.getFullYear(),
                $scope.From_Date.getMonth(),
                $scope.From_Date.getDate());
            $scope.toDate = $scope.tominDate;

            $scope.minDate = new Date(
                $scope.toDate.getFullYear(),
                $scope.toDate.getMonth(),
                $scope.toDate.getDate() + 1);
            $scope.maxDate = new Date(
                $scope.toDate.getFullYear(),
                $scope.toDate.getMonth(),
                $scope.toDate.getDate() + 1);
            angular.forEach($scope.gridleavestatus.data, function (obj) {
                if (obj.hrelaP_Id === row.entity.hrelaP_Id) {
                    obj.hrelaP_ReportingDate = $scope.minDate;
                }
            });
        };

        $scope.formatString = function (format) {
            var day = parseInt(format.substring(0, 2));
            var month = parseInt(format.substring(3, 5));
            var year = parseInt(format.substring(6, 10));
            var date = new Date(year, month - 1, day);
            return date;
        };

        $scope.viewdocument = function (obj) {
            if (obj != null) {
                var img = obj.HRELAP_SupportingDocument;
                var imagarr = img.split('.');
                var extention = imagarr[imagarr.length - 1];
                obj.filetype = extention;
                if (extention === 'doc' || extention === 'docx' || extention === 'ppt' || extention === 'pptx' || extention === 'xlsx' || extention === 'xls') {
                    dd.document_Pathnew = "https://view.officeapps.live.com/op/view.aspx?src=" + obj.HRELAP_SupportingDocument;
                }
                else if (extention === "jpg" || extention === "jpeg" || extention === "JPEG") {
                    $('#preview').removeAttr('src');
                    $('#preview').attr('src', obj.HRELAP_SupportingDocument);
                    $('#myModal').modal();
                    $('#myModal').modal({ keyboard: false });
                    $('#myModal').modal('show');
                }
                else if (extention === "pdf") {
                    var imagedownload = obj.HRELAP_SupportingDocument;
                    $scope.content = "";
                    var fileURL = "";
                    var file = "";
                    $http.get(imagedownload, { responseType: 'arraybuffer' })
                        .success(function (response) {
                            file = new Blob([(response)], { type: 'application/pdf' });
                            fileURL = URL.createObjectURL(file);
                            $scope.content = $sce.trustAsResourceUrl(fileURL);
                            $('#showpdf').modal('show');
                        });
                }
            }
        };

        $scope.viewcomment = function (obj) {
            $scope.commentlist = [];
            $scope.deviationlist = [];
            $scope.HRME_EmployeeFirstName = "";
            $scope.HRELAP_LeaveReason = "";
            $scope.HRELAP_TotalDays = "";

            var data = {
                "HRELAP_Id": obj.HRELAP_Id,
                "HRME_Id": obj.HRME_Id
            };
            apiService.create("LeaveApproval/viewcomment", data).then(function (promise) {
                $scope.HRME_EmployeeFirstName = obj.HRME_EmployeeFirstName;
                $scope.HRELAP_LeaveReason = obj.HRELAP_LeaveReason;
                $scope.HRELAP_TotalDays = obj.HRELAP_TotalDays;
                $scope.commentlist = promise.commentlist;
                $scope.deviationlist = promise.deviationdetails;
            });
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

        $scope.cancel = function () {
            $state.reload();
        };

        $scope.GetLeave = function (leave) {
            if (leave.selected) {
                $scope.rowsSelected.push(leave);
            }
            else {
                const ind = $scope.rowsSelected.indexOf(leave);
                if (ind > -1) {
                    $scope.rowsSelected.splice(ind, 1);
                }
            }
        }

        $scope.toggleAll = function () {
            var toggleStatus = $scope.all;
            angular.forEach($scope.get_leavestatus, function (itm) {
                itm.selected = toggleStatus;
                if ($scope.all == true) {
                    if ($scope.rowsSelected.indexOf(itm) === -1) {
                        $scope.rowsSelected.push(itm);
                    }
                }
                else {
                    $scope.rowsSelected.splice(itm);
                }
            });
        }

        $scope.isOptionsRequired = function () {
            return !$scope.get_leavestatus.some(function (options) {
                return options.selected;
            });
        };

        $scope.ViewRemarksLevelWise = function (dd) {
            $scope.Getviewremarksdetails = [];
            $scope.HRME_EmployeeFirstName = "";
            $scope.HRELAP_LeaveReason = "";
            $scope.HRELAP_TotalDays = "";
            $scope.HRML_LeaveName = "";
            var data = {
                "HRELAP_Id": dd.hrelaP_Id,
                "HRELT_Id": dd.HRELT_Id,
                "HRME_Id": dd.hrmE_Id,
            };

            apiService.create("LeaveApproval/ViewRemarksLevelWise", data).then(function (promise) {
                if (promise !== null) {
                    $scope.Getviewremarksdetails = promise.getviewremarksdetails;
                    if ($scope.Getviewremarksdetails !== null && $scope.Getviewremarksdetails.length > 0) {
                        $scope.trustAsHtml = function (html) {
                            return $sce.trustAsHtml(html);
                        };
                        $scope.HRME_EmployeeFirstName = dd.hrmE_EmployeeFirstName;
                        $scope.HRELAP_LeaveReason = dd.hrelaP_LeaveReason;
                        $scope.HRELAP_TotalDays = dd.hrelT_TotDays;
                        $scope.HRML_LeaveName = dd.hrmL_LeaveName;
                        $('#viewremarksstatus').modal('show');
                    }
                }
            });
        };

        $scope.todate = function (secondDate1, row) {
            $scope.ReportingDate = secondDate1;
            var firstDate1 = $filter('date')(row.HRELAP_FromDate, "dd/MM/yy");
            secondDate1 = $filter('date')(secondDate1, "dd/MM/yy");
            var date2 = new Date($scope.formatString(secondDate1));
            var date1 = new Date($scope.formatString(firstDate1));
            var timeDiff = Math.abs(date2.getTime() - date1.getTime());
            $scope.dayDifference = Math.ceil(timeDiff / (1000 * 3600 * 24));
            row.HRELAP_TotalDaysapprv = $scope.dayDifference + 1;
            //angular.forEach($scope.get_leavestatus, function (obj) {
            //    if (obj.hrelaP_Id === row.hrelaP_Id) {
            //    }
            //});
        };


        $scope.Viewleavebalancehistory = function (dd) {

            $scope.getemployeeleavedetails = [];
            $scope.getemployeehistroyleavedetails = [];
            $scope.HRME_EmployeeFirstName_Temp = dd.HRME_EmployeeFirstName;
            $scope.HRME_EmployeeFirstName = dd.HRME_EmployeeFirstName;
            $scope.itemsPerPage10 = 10;
            $scope.currentPage10 = 1;
            var data = {
                "HRML_Id": dd.HRML_Id,
                "HRME_Id": dd.HRME_Id
            };
            apiService.create("LeaveApproval/Viewleavebalancehistory", data).then(function (promise) {
                if (promise !== null) {
                    $scope.getemployeeleavedetails = promise.getemployeeleavedetails;
                    $scope.getemployeehistroyleavedetails = promise.getemployeehistroyleavedetails;
                    $scope.HRELAP_LeaveReason = dd.HRELAP_LeaveReason;
                    $scope.HRELAP_TotalDays = dd.HRELAP_TotalDays;
                    $scope.HRML_LeaveName  = dd.HRML_LeaveName ;
                }

            });
        };


        $scope.proxydetials = function (dd) {
            $scope.HRMDES_DesignationName = "";
            $scope.getemployeeleavedetails = [];
            $scope.getemployeehistroyleavedetails = [];
            $scope.HRME_EmployeeFirstName_Temp = dd.HRME_EmployeeFirstName;
            $scope.HRME_EmployeeFirstName = dd.HRME_EmployeeFirstName;
            $scope.ProxyName = dd.ProxyName;
            $scope.HRMDES_DesignationName = dd.HRMDES_DesignationName;
            $scope.itemsPerPage10 = 10;
            $scope.currentPage10 = 1;
            var data = {
                "HRML_Id": dd.HRML_Id,
                "HRME_Id": dd.HRME_Id
            };
            apiService.create("LeaveApproval/Viewleavebalancehistory", data).then(function (promise) {
                if (promise !== null) {
                    $scope.getemployeeleavedetails = promise.getemployeeleavedetails;
                    $scope.getemployeehistroyleavedetails = promise.getemployeehistroyleavedetails;
                    $scope.HRELAP_LeaveReason = dd.HRELAP_LeaveReason;
                    $scope.HRELAP_TotalDays = dd.HRELAP_TotalDays;
                    $scope.HRML_LeaveName = dd.HRML_LeaveName;
                    $scope.proxyempdetials(dd);
                }

            });
        };

        $scope.proxyempdetials = function (dd) {
            
            var data = {
                "HRMEIdproxy": dd.ProxyHRMEID,
                "HRELAP_FromDate":new Date(dd.HRELAP_FromDate).toDateString(),
                "HRELAP_ToDate": new Date(dd.HRELAP_ToDate).toDateString(),
                "HRME_Id": dd.HRME_Id

            };
            apiService.create("OnlineLeaveApplication/proxytaskdetails", data).then(function (promise) {
                $scope.proxytaskList = promise.proxytaskList;
                if (promise.proxytaskList != null && promise.proxytaskList != undefined && promise.proxytaskList != 0) {
                    $scope.proxytaskList = promise.proxytaskList;
                }
                else {
                    swal('Data not Found !!');
                }
            })
        }

    }
})();