(function () {
    'use strict';
    angular
        .module('app')
        .controller('HREmployeeLoanApprovalController', HREmployeeLoanApprovalController)

    HREmployeeLoanApprovalController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$stateParams', '$filter', 'superCache']
    function HREmployeeLoanApprovalController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $stateParams, $filter, superCache) {
        //object
        $scope.currentPage = 10;
        $scope.itemsPerPage = 10;

        //Employeee

        // Get form Details at onload 
        $scope.onLoadGetData = function () {
            var pageid = 2;
            apiService.getURI("HREmployeeLoanApproval/getalldetails", pageid).then(function (promise) {

                if (promise.leaveyeardropdown !== null && promise.leaveyeardropdown.length > 0) {

                    $scope.leaveyeardropdown = promise.leaveyeardropdown;
                }

                if (promise.monthdropdown !== null && promise.monthdropdown.length > 0) {
                    $scope.monthdropdown = promise.monthdropdown;


                }
                if (promise.companydetails !== null && promise.companydetails.length > 0) {
                    $scope.companydetails = promise.companydetails;
                }


                if (promise.employeedropdown !== null && promise.employeedropdown.length > 0) {
                    $scope.employeedropdown = promise.employeedropdown;
                }


                if (promise.groupTypedropdown !== null && promise.groupTypedropdown.length > 0) {
                    $scope.groupTypedropdown = promise.groupTypedropdown;
                    $scope.groupTypeselectedAll = true;
                    $scope.GetEmployeeBygroupTypeAll($scope.groupTypeselectedAll);

                }


                if (promise.departmentdropdown !== null && promise.departmentdropdown.length > 0) {
                    $scope.departmentdropdown = promise.departmentdropdown;
                    $scope.departmentselectedAll = true;
                    $scope.GetEmployeeByDepartmentAll($scope.departmentselectedAll);
                }

                if (promise.designationdropdown !== null && promise.designationdropdown.length > 0) {
                    $scope.designationdropdown = promise.designationdropdown;

                    $scope.designationselectedAll = true;
                    $scope.GetEmployeeByDesignationAll($scope.designationselectedAll);


                }

                if (promise.configurationDetails != null) {

                    $scope.SalaryFromDay = promise.configurationDetails.hrC_SalaryFromDay;
                    $scope.SalaryToDay = promise.configurationDetails.hrC_SalaryToDay;

                }


               

                if (promise.salaryapprovedgridview !== null && promise.salaryapprovedgridview.length > 0) {
                    $scope.salaryapprovedgridview = promise.salaryapprovedgridview;
                }

                if (promise.employeeSalaryslipDetails !== null && promise.employeeSalaryslipDetails.length > 0) {
                    $scope.EmployeeDis = true;

                    $scope.employeeSalaryslipDetails = promise.employeeSalaryslipDetails;


                    angular.forEach($scope.employeeSalaryslipDetails, function (itm) {

                        if (promise.finalflag != null) {
                            $scope.finalflag = promise.finalflag;
                            itm.HREL_SactionedAmount = itm.HREL_LoanAmount

                        }

                        if (itm.prevremarks == undefined) {
                            $scope.prevremarksflag = 0;

                        }
                        else {
                            $scope.prevremarksflag = $scope.prevremarksflag + 1;
                        }
                    });


                    $scope.grossEarning = 0;
                    $scope.grossDeduction = 0;
                    $scope.TotalSalary = 0;
                    $scope.alltask = true;
                    $scope.all_task($scope.alltask)


                }
                //else {
                //    $scope.EmployeeDis = false;
                //    swal('No Record found to display .. !');
                //    return;
                //}



              





            })
        }

        $scope.salaryapprovedgridview = [];
        $scope.salaryholddgridview = [];
        //




        $scope.employeeSalaryslipDetails = [];
        $scope.institutionDetails = {};
        $scope.EmployeeDis = false;
        //Search employee
        $scope.submitted = false;


        $scope.monthdays = "";
        $scope.earningheadlist = [];
        $scope.deductionheadlist = [];
        $scope.employeeSalaryslipDetails = [];

        $scope.SearchEmployee = function () {
            $scope.EmployeeDis = false;
            $scope.submitted = true;
            $scope.earningheadlist = [];
            $scope.deductionheadlist = [];
            $scope.employeeSalaryslipDetails = [];
            if ($scope.myForm.$valid) {
                $scope.monthdays = "";
                if ($scope.monthdropdown != null && $scope.monthdropdown.length > 0) {
                    angular.forEach($scope.monthdropdown, function (itm) {
                        if (itm.ivrM_Month_Name == $scope.Employee.hreS_Month) {
                            // monthdays.push(itm.hrmgT_Id);//
                            $scope.monthdays = Number(itm.ivrM_Month_Max_Days);
                        }

                    });
                }



                $scope.institutionDetails = {};
                var groupTypeselected = [];
                angular.forEach($scope.groupTypedropdown, function (itm) {
                    if (itm.selected) {
                        groupTypeselected.push(itm.hrmgT_Id);//
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

                if (groupTypeselected.length == 0 && departmentselected.length == 0 && designationselected.length == 0) {
                    swal('Kindly select atleast one record');
                    return;
                }


                var data = {
                    "HRES_Year": $scope.Employee.hreS_Year,
                    "HRES_Month": $scope.Employee.hreS_Month,
                    "institutionID": $scope.Employee.institutionID,
                    groupTypeIdList: groupTypeselected,
                    hrmD_IdList: departmentselected,
                    hrmdeS_IdList: designationselected,
                    "comm": "1"
                }

                apiService.create("HREmployeeLoanApproval/getapprvaldata", data).
                    then(function (promise) {

                       


                        if (promise.finalflag != null) {
                            $scope.finalflag = promise.finalflag;

                        }
                    
               

                        if (promise.employeeSalaryslipDetails !== null && promise.employeeSalaryslipDetails.length > 0) {
                            $scope.EmployeeDis = true;

                            $scope.employeeSalaryslipDetails = promise.employeeSalaryslipDetails;


                            angular.forEach($scope.employeeSalaryslipDetails, function (itm) {
                                if (itm.prevremarks == undefined) {
                                    $scope.prevremarksflag = 0;

                                }
                                else {
                                    $scope.prevremarksflag = $scope.prevremarksflag + 1;
                                }
                            });


                            $scope.grossEarning = 0;
                            $scope.grossDeduction = 0;
                            $scope.TotalSalary = 0;
                            $scope.alltask = true;
                            $scope.all_task($scope.alltask)


                        }
                        else {
                            $scope.EmployeeDis = false;
                            swal('No Record found to display .. !');
                            return;
                        }

                    })
            }

        }

        $scope.usercheckC2 = "";
        $scope.get_evalistt = function (user) {
            $scope.usercheckC2 = $scope.employeeSalaryslipDetails.every(function (options) {
                return options.selectedd2;
            });


            angular.forEach($scope.employeeSalaryslipDetails, function (itm) {
                if (user.selectedd2 == false) {
                    user.HRESA_Remarks = "";
                }
                else {
                    user.HRESA_Remarks = "Approved";
                }
            });


        };

        $scope.selectedAll1 = "";
        $scope.all_task = function (alltask) {
            var checkStatus2 = alltask;
            angular.forEach($scope.employeeSalaryslipDetails, function (itm) {
                itm.selectedd2 = checkStatus2;

                if (itm.selectedd2 == false) {
                    itm.HRESA_Remarks = "";
                }
                else {
                    itm.HRESA_Remarks = "Approved";
                }
            });
        };



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
        }

        //single
        $scope.GetEmployeeBygroupType = function (groupType) {
            $scope.departmentdropdown = [];
            $scope.designationdropdown = [];
            $scope.designationselectedAll = false;
            $scope.departmentselectedAll = false;

            if ($scope.EmployeeDis) {
                $scope.EmployeeDis = false;
            }
            $scope.groupTypeselectedAll = $scope.groupTypedropdown.every(function (itm) {

                return itm.selected;
            });

            $scope.get_depts();
        };
        $scope.get_depts = function () {
            var ids = [];
            angular.forEach($scope.groupTypedropdown, function (grp_t) {
                if (grp_t.selected) {
                    ids.push(grp_t.hrmgT_Id);
                }
            })
            var data = {
                hrmgT_IdList: ids
            }
            apiService.create("HREmployeeLoanApproval/get_depts", data).
                then(function (promise) {

                    if (promise.departmentdropdown !== null && promise.departmentdropdown.length > 0) {
                        $scope.departmentdropdown = promise.departmentdropdown;
                        $scope.departmentselectedAll = true;
                        $scope.GetEmployeeByDepartmentAll($scope.departmentselectedAll);
                    }
                })
        };



        //By Department
        $scope.GetEmployeeByDepartmentAll = function (departmentselectedAll) {
            if ($scope.EmployeeDis) {
                $scope.EmployeeDis = false;
            }

            var toggleStatus = $scope.departmentselectedAll;
            angular.forEach($scope.departmentdropdown, function (itm) {
                itm.selected = toggleStatus;
            });
            angular.forEach($scope.designationdropdown, function (itm1) {
                itm1.selected = toggleStatus;
                $scope.designationselectedAll = toggleStatus;

            })

            $scope.get_desig();

        }

        //GET ALL SELECT
        $scope.GetEmployeeAll = function (employee) {
            var toggleStatus = $scope.employeeSelectedAll;
            angular.forEach($scope.employeeSalaryslipDetails, function (itm) {
                itm.selected = toggleStatus;
            });
        };

        $scope.GetEmployee = function (employee) {

            $scope.employeeSelectedAll = $scope.employeeSalaryslipDetails.every(function (itm) {
                return itm.selected;
            });

            if (employee.selected) {
                employee.disable = false;
            }
            else {

                employee.disable = true;
            }

        }
        //By Department Single
        $scope.GetEmployeeByDepartment = function (department) {
            $scope.designationdropdown = [];
            $scope.designationselectedAll = false;
            if ($scope.EmployeeDis) {
                $scope.EmployeeDis = false;
            }
            $scope.departmentselectedAll = $scope.departmentdropdown.every(function (itm) {

                return itm.selected;
            });
            $scope.get_desig();
        }
        $scope.get_desig = function () {
            var ids = [];
            angular.forEach($scope.groupTypedropdown, function (grp_t) {
                if (grp_t.selected) {
                    ids.push(grp_t.hrmgT_Id);
                }
            })
            var ids1 = [];
            angular.forEach($scope.departmentdropdown, function (grp_t) {
                if (grp_t.selected) {
                    ids1.push(grp_t.hrmD_Id);
                }
            })
            var data = {
                hrmgT_IdList: ids,
                hrmD_IdList: ids1
            }
            apiService.create("HREmployeeLoanApproval/get_desig", data).
                then(function (promise) {
                    if (promise.designationdropdown !== null && promise.designationdropdown.length > 0) {
                        $scope.designationdropdown = promise.designationdropdown;
                        $scope.designationselectedAll = true;
                        $scope.GetEmployeeByDesignationAll($scope.designationselectedAll);
                    }
                })
        };


        $scope.SetMonthByYear = function () {

            $scope.salaryapprovedgridview = [];
            $scope.salaryholddgridview = [];
            $scope.employeeSalaryslipDetails = [];
            $scope.HRES_Year = $scope.Employee.hreS_Year;
            $scope.HRES_Month = $scope.Employee.hreS_Month;

            if ($scope.Employee.hreS_Year.length > 0 && $scope.Employee.hreS_Month.length > 0) {
                var data = {
                    "HRES_Year": $scope.Employee.hreS_Year,
                    "HRES_Month": $scope.Employee.hreS_Month,
                    "institutionID": $scope.Employee.institutionID,
                }
            }

            apiService.create("HREmployeeLoanApproval/SalaryApprovalgriddata", data).then(function (promise) {
                if (promise.salaryapprovedgridview !== null && promise.salaryapprovedgridview.length > 0) {
                    $scope.salaryapprovedgridview = promise.salaryapprovedgridview;
                }
                if (promise.salaryholddgridview !== null && promise.salaryholddgridview.length > 0) {
                    $scope.salaryholddgridview = promise.salaryholddgridview;
                }
            })
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

        }


        //By Designation Single
        $scope.GetEmployeeByDesignation = function (designation) {
            if ($scope.EmployeeDis) {
                $scope.EmployeeDis = false;
            }
            $scope.designationselectedAll = $scope.designationdropdown.every(function (itm) {

                return itm.selected;
            });
        }


        //Clear data
        $scope.Employee = {};
        $scope.cleardata = function () {
            $state.reload();

        }

        $scope.interacted = function (field) {
            return $scope.submitted || field.$dirty;
        };

        $scope.UpdateEmployeeDetails = function (msg) {
            $scope.submitted = true;
            var msg1 = '';
            if ($scope.myForm.$valid) {
                $scope.employeeselected = [];
                $scope.employeeselectedfalserec = [];
                $scope.employeeselectedapproved = [];

                var checkStatus2 = $scope.alltask;
                angular.forEach($scope.employeeSalaryslipDetails, function (emp) {
                    if (emp.selectedd2 === true) {
                        $scope.employeeselected.push({ HRES_Id: emp.HREL_Id, HRESA_Remarks: emp.HRESAA_Remarks, retrunMsg: msg, HREL_SanctionedAmount: Number(emp.HREL_SactionedAmount), HRME_Id: emp.HRME_Id});

                    }
                   
                });

                //angular.forEach($scope.employeeSalaryslipDetails, function (emp) {
                //    if (emp.selectedd2 === false) {
                //        $scope.employeeselectedfalserec.push({ HRES_Id: emp.HRES_Id, HRESA_Remarks: emp.HRESA_Remarks });
                //    }
                //});

                if (msg == "Approved") {
                    msg1 = 'Approve';
                }
                else {
                    msg1 = 'Reject';
                }


                swal({
                    title: "Are you sure?",
                    //text: "Do You Want To Updated The Record..",
                    text: "Do You Want To " + msg,
                    type: "warning",
                    showCancelButton: true,
                    confirmButtonColor: "#DD6B55",
                    confirmButtonText: "Yes, Update it!",
                    cancelButtonText: "Cancel..!",
                    closeOnConfirm: false,
                    closeOnCancel: false
                },
                    function (isConfirm) {
                        if (isConfirm) {



                            if ($scope.employeeselected.length > 0) {
                                var data = {
                                    //"comm": "Approved",
                                    selectedEmpdetails: $scope.employeeselected,
                                   // "HRES_Year": $scope.Employee.hreS_Year,
                                    //"HRES_Month": $scope.Employee.hreS_Month,
                                    "institutionID": $scope.Employee.institutionID
                                };
                                apiService.create("HREmployeeLoanApproval/SalaryApproval", data).
                                    then(function (promise) {
                                        $scope.returnmsg = "Updated";


                                        if (promise.retrunMsg !== "") {
                                            if (promise.retrunMsg == "false") {
                                                // swal("Record Not Updated..");
                                                $scope.returnmsg = "NotUpdated";
                                                return;
                                            }
                                            else if (promise.retrunMsg == "updated") {
                                                $scope.returnmsg = "Updated";

                                               
                                                swal("Record Saved Successfully..");


                                                    $scope.cleardata();

                                               


                                            }
                                            else if (promise.retrunMsg == "") {
                                                // swal("You are not allowed to update Salary");

                                            }

                                        }
                                        else {
                                            $scope.EmployeeDis = false;
                                            //swal('No Record found to Update .. !');
                                            return;
                                        }


                                    })
                            }
                            //if ($scope.employeeselectedfalserec.length > 0) {

                            //    var data = {
                            //        "comm": "Hold",
                            //        selectedEmpdetails: $scope.employeeselectedfalserec,
                            //        "HRES_Year": $scope.Employee.hreS_Year,
                            //        "HRES_Month": $scope.Employee.hreS_Month
                            //    };
                            //    apiService.create("HREmployeeLoanApproval/SalaryApproval", data).
                            //        then(function (promise) {

                            //            if (promise.retrunMsg !== "") {
                            //                if (promise.retrunMsg == "false") {
                            //                    // swal("Record Not Updated..");
                            //                    returnmsg = 0;
                            //                    return;
                            //                }
                            //                else if (promise.retrunMsg == "updated") {
                            //                    //swal("Record Updated Successfully..");
                            //                    returnmsg = 1;


                            //                }
                            //                else if (promise.retrunMsg == "Configuration Message") {
                            //                    // swal("You are not allowed to update Salary");

                            //                }

                            //            }
                            //            else {
                            //                $scope.EmployeeDis = false;
                            //                //swal('No Record found to Update .. !');
                            //                return;
                            //            }
                            //        })
                            //}





                        }
                        else {
                            swal("Cancelled");
                            // $state.reload();
                        }
                    });





            }
            else {
                swal("Please Enter Remarks");
            }
        }


        $scope.printData = function () {
            var divToPrint = document.getElementById("Table");
            var newWin = window.open();
            newWin.document.write(divToPrint.outerHTML);
            newWin.print();
            newWin.close();
            // $state.reload();
        }

        //Total for per column

        $scope.TotalgrossEarning = function () {
            var total = 0;
            if ($scope.employeeSalaryslipDetails != null) {
                for (var i = 0; i < $scope.employeeSalaryslipDetails.length; i++) {


                    var product = $scope.employeeSalaryslipDetails[i];
                    total += product.grossEarning;
                }
            }

            return Math.round(total);
        }

        $scope.TotalgrossDeduction = function () {
            var total = 0;
            if ($scope.employeeSalaryslipDetails != null) {
                for (var i = 0; i < $scope.employeeSalaryslipDetails.length; i++) {


                    var product = $scope.employeeSalaryslipDetails[i];
                    total += product.grossDeduction;
                }
            }

            return Math.round(total);
        }

        $scope.TotalnetSalary = function () {
            var total = 0;
            if ($scope.employeeSalaryslipDetails != null) {
                for (var i = 0; i < $scope.employeeSalaryslipDetails.length; i++) {


                    var product = $scope.employeeSalaryslipDetails[i];
                    total += product.netSalary;
                }
            }

            return Math.round(total);
        }



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
        }


        $scope.ViewHealthCheckUpFiles = function (obj) {


            var data = {
                "HREL_Id": obj.HREL_Id,

            };

            apiService.create("HREmployeeLoanApproval/ViewFiles", data).then(function (promise) {
                if (promise !== null) {
                    if (promise.getviewfilesdetails !== null && promise.getviewfilesdetails.length > 0) {
                        $scope.getviewfilesdetails = promise.getviewfilesdetails;
                        angular.forEach($scope.getviewfilesdetails, function (dd) {
                            if (dd.hrelD_Filepath !== null && dd.hrelD_Filepath !== "") {
                                var img = dd.hrelD_Filepath;
                                var imagarr = img.split('.');
                                var lastelement = imagarr[imagarr.length - 1];
                                dd.filetype = lastelement;
                                console.log("dd.filetype : " + dd.filetype);
                                if (lastelement === 'doc' || lastelement === 'docx' || lastelement === 'ppt' || lastelement === 'pptx' || lastelement === 'xlsx' || lastelement === 'xls') {
                                    dd.document_Pathnew = "https://view.officeapps.live.com/op/view.aspx?src=" + dd.hrelD_Filepath;
                                }
                            }
                        });
                        $('#modalviewfiles').modal('show');
                    } else {
                        swal("No Records Found");
                        $('#modalviewfiles').modal('hide');
                    }


                }
            });
        };

        $scope.showmothersign = function (img) {
            
            $scope.imagepreview = img;
            $scope.view_videos = [];
            var img = $scope.imagepreview;
            if (img != null) {
                var imagarr = img.split('.');
                var lastelement = imagarr[imagarr.length - 1];
                $scope.filetype2 = lastelement;
            }
            if ($scope.filetype2 == 'wmv' || $scope.filetype2 == 'mp4') {

                $scope.view_videos.push({ id: 1, ihw_video: img });
                $('#myvideoPreview').modal('show');

            }
            else if ($scope.filetype2 == 'jpg' || $scope.filetype2 == 'jpeg' || $scope.filetype2 == 'png') {

                $('#preview').attr('src', $scope.imagepreview);
                $('#myModalimg').modal('show');

            }
            else if ($scope.filetype2 == 'doc' || $scope.filetype2 == 'docx' || $scope.filetype2 == 'xls' || $scope.filetype2 == 'xlsx' || $scope.filetype2 == 'ppt' || $scope.filetype2 == 'pptx') {
                $window.open($scope.imagepreview)
            }
            else if ($scope.filetype2 == 'mp3') {
                $scope.view_videos.push({ id: 1, ihw_video: img });
                $('#myaudioPreview').modal('show');

            }
            else if ($scope.filetype2 == 'pdf') {

                ///=====================show pdf, img

                $('#showpdf').modal('hide');
                var imagedownload1 = "";
                imagedownload1 = $scope.imagepreview;


                $http.get(imagedownload1, { responseType: 'arraybuffer' })
                    .success(function (response) {
                        var fileURL = "";
                        var file = "";
                        var embed = "";
                        var pdfId = "";
                        file = new Blob([(response)], { type: 'application/pdf' });
                        fileURL = URL.createObjectURL(file);

                        pdfId = document.getElementById("pdfIdzz");
                        pdfId.removeChild(pdfId.childNodes[0]);
                        embed = document.createElement('embed');
                        embed.setAttribute('src', fileURL);
                        embed.setAttribute('type', 'application/pdf');
                        embed.setAttribute('width', '100%');
                        embed.setAttribute('height', '1000');
                        pdfId.appendChild(embed);
                        $('#showpdf').modal('show');



                    });



            }
            else {
                $window.open($scope.imagepreview)
            }
        };


        $scope.downloaddirectimage = function (data, idd, type) {
            var studentreg = idd;
            $scope.imagedownload = data;
            var imagedownload = data;
            $http.get(imagedownload, { responseType: "arraybuffer" }).success(function (data) {
                var anchor = angular.element('<a/>');
                var blob = new Blob([data]);
                anchor.attr({
                    href: window.URL.createObjectURL(blob),
                    target: '_blank',
                    download: studentreg
                })[0].click();
            });
        };


    }


})();