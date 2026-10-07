(function () {
    'use strict';
    angular
        .module('app')
        .controller('HR_Planner_ManagementClientRemindercontroller', HR_Planner_ManagementClientRemindercontroller)

    HR_Planner_ManagementClientRemindercontroller.$inject = ['$rootScope', '$scope', '$state', '$location', 'apiService', 'Flash', '$http', '$q', '$stateParams', '$filter', '$sce', 'Excel', '$timeout','$compile']
    function HR_Planner_ManagementClientRemindercontroller($rootScope, $scope, $state, $location, apiService, Flash, $http, $q, $stateParams, $filter, $sce, Excel, $timeout,$compile) {
        $scope.editflag = false;
        $scope.submitted = false;
        $scope.setremind = false;
        $scope.disabledata = false;
        $scope.presentCountgrid = 0;
        $scope.obj = {};
        $scope.getserverdate = function () {
            var xmlHttp;
            function srvTime() {
                try {
                    //FF, Opera, Safari, Chrome
                    xmlHttp = new XMLHttpRequest();
                }
                catch (err1) {
                    //IE
                    try {
                        xmlHttp = new ActiveXObject('Msxml2.XMLHTTP');
                    }
                    catch (err2) {
                        try {
                            xmlHttp = new ActiveXObject('Microsoft.XMLHTTP');
                        }
                        catch (eerr3) {
                            //AJAX not supported, use CPU time.
                            alert("AJAX not supported");
                        }
                    }
                }
                xmlHttp.open('HEAD', window.location.href.toString(), false);
                xmlHttp.setRequestHeader("Content-Type", "text/html");
                xmlHttp.send('');
                return xmlHttp.getResponseHeader("Date");
            }
            $scope.today = srvTime();
            $scope.today = new Date($scope.today);
        }
        $scope.getserverdate();
        $scope.hrplmG_EntryDateTime = new Date($scope.today);
        $scope.setremind = true;
        $scope.loaddata = function () {
            $scope.editflag = false;
            $scope.currentPage = 1;
            $scope.itemsPerPage = 10;
            $scope.search = "";
            var pageid = 2;
            apiService.getURI("HR_Planner_Management/getdetails", pageid).then(function (promise) {
                 $scope.griddata = promise.griddata;
                
                $scope.employeelisttable = promise.employeelistdata;
                $scope.multipleuserlevel = [{ id: 'Materal1', hrplmgapP_ApprovalAlevel: '1', employeelistdata: $scope.employeelisttable }];
                if ($scope.griddata != null) {
                    $scope.disabledata = true;
                    $scope.presentCountgrid = $scope.griddata.length;
                }
                $scope.employeelist = promise.employeelist;
                $scope.Clientlist = promise.clientNames;
               
                $scope.clickreminder();

            });
        };
        $scope.validate = false;
        $scope.onflagchange = function (flg) {
            if (flg == "Never") {
                $scope.hrplmG_ReminderFlag = "None";
            }
            else {
                $scope.hrplmG_ReminderFlag = "";
            }

        };
        $scope.htmlText = "";
        //pendingissues
        $scope.ViewContent = function (View) {
            $scope.htmlText = "";
            //var e1 = angular.element(document.getElementById("testfollowup"));
            //$compile(e1.html(View.hrplmG_ReminderContent))(($scope));
            $scope.htmlText = View.hrplmG_ReminderContent;
            $('#popup1111').modal('show');
        }
        $scope.loaddatareminder = function () {
            $scope.disabledata = false; $scope.griddata = [];
            $scope.currentPage = 1;
            $scope.itemsPerPage = 10;
            $scope.search = "";
            var pageid = 2;
            apiService.getURI("HR_Planner_Management/getdetailsreminder", pageid).then(function (promise) {
              //  $scope.griddata = promise.griddata;
                if (promise.griddata != null && promise.griddata.length > 0) {
                    angular.forEach(promise.griddata, function (itm) {
                        if (itm.ISMMCLT_Id > 0) {
                            $scope.griddata.push(itm);
                        }

                    });
                }
                if ($scope.griddata != null) {
                    $scope.disabledata = false;
                    $scope.presentCountgrid = $scope.griddata.length;
                }
                
                $scope.employeelist = promise.employeelist;
            });
        };

        $scope.sort = function (keyname) {
            $scope.sortKey = keyname;
            $scope.reverse = !$scope.reverse;
        };

        $scope.clickreminder = function () {

            $scope.griddata = [];
            if ($scope.setremind == false) {
                $scope.loaddata();
            }
            else {
                $scope.loaddatareminder();
            }


        }
        //$scope.exportToExcel = function () {
        //    var divToPrint = document.getElementById("printDeviation");


        //    var exportHref = Excel.tableToExcel(divToPrint, divToPrint);
        //    $timeout(function () {
        //        location.href = exportHref;
        //    }, 100);


        //};

        $scope.OnchageInvoice = function (dd) {
            $scope.InvoiceDetails = [];
            angular.forEach($scope.Performadetails, function (cls) {
                if (cls.ISMPRINC_Id == $scope.ISMPRINC_Id) {
                    $scope.InvoiceDetails.push(cls);
                }
            });

        }
        $scope.SaveModal = function () {
            $scope.HRPLMG_ReminderContent = "";
           // let html = text.replace(/\n/g, '<br>').replace(/^\s*-\s/gm, '<li>').replace(/\n(?=\s*<li>)/g, '</li>').replace(/\s*-\s/g, '</li><li>');
            $scope.HRPLMG_ReminderContent = "";    
            $scope.text = document.getElementById("LeadIntimate").innerHTML;;
            let html = $scope.text.replace(/\n/g, '<br>').replace(/^\s*-\s/gm, '<li>').replace(/\n(?=\s*<li>)/g, '</li>').replace(/\s*-\s/g, '</li><li>');
            $scope.HRPLMG_ReminderContent = document.getElementById("LeadIntimate").innerHTML;;;
            $('#myModal1').modal('hide');

        }
        //OnchageTemplate
        $scope.OnTemplatename = function () {
            if ($scope.ISMPRINC_Id > 0) {
                if ($scope.VMSMOUT_Id > 0) {
                    angular.forEach($scope.Templatedetails, function (cls) {
                        if (cls.VMSMOUT_Id == $scope.VMSMOUT_Id) {
                            var e1 = angular.element(document.getElementById("LeadIntimate"));
                            $compile(e1.html(cls.VMSMOUT_Template))(($scope));
                            $('#myModal1').modal('show');
                            return;
                        }
                    });
                  
                }
                else {
                    swal("Please Select  Template !");
                    $scope.VMSMOUT_Id = "";
                }
                
            }
            else {
                swal("Please Select  Invoice !");
                $scope.VMSMOUT_Id = "";
            }

        }
        //Client data
      
        $scope.get_clientDetails = function (ISMMCLT_Id) {
            $scope.Performadetails = []; $scope.PerformadetailsTemp = [];
             //$scope.hrplmG_PlannerName = "";
            $scope.Templatedetails = []; $scope.VMSMOUT_Id = ""; $scope.ISMPRINC_Id = "";
            var data = {
                "ISMMCLT_Id": ISMMCLT_Id.ISMMCLT_Id
            }
            apiService.create("HR_Planner_Management/clientDetails", data).then(function (promise) {
                if (promise.performadetails !== null && promise.performadetails.length > 0) {
                     $scope.PerformadetailsTemp = promise.performadetails;
                    $scope.Templatedetails = promise.templatedetails;
                    //angular.forEach($scope.Clientlist, function (cls) {
                    //    if (cls.ISMMCLT_Id === ISMMCLT_Id.ISMMCLT_Id) {
                    //        $scope.hrplmG_PlannerName = cls.ISMMCLT_ClientName;
                    //        return;
                    //    }
                    //});
                    //
                    $scope.Performadetails = [];
                    angular.forEach(promise.performadetails, function (dev) {
                        if ($scope.Performadetails.length === 0) {
                            $scope.Performadetails.push(dev);
                        }
                        else if ($scope.Performadetails.length > 0) {
                            var intcount = 0;
                            angular.forEach($scope.Performadetails, function (emp) {
                                if (emp.ISMPRINC_Id === dev.ISMPRINC_Id) {
                                    intcount += 1;
                                }
                            });
                            if (intcount === 0) {
                                $scope.Performadetails.push(dev);
                            }
                        }
                    });

                    ///InvoiceMultiple
                    if ($scope.Performadetails != null && $scope.Performadetails.length > 0) {
                        angular.forEach($scope.Performadetails, function (cls) {
                            cls.invoiceMode = [];
                            angular.forEach(promise.performadetails, function (dd) {
                                if (cls.ISMPRINC_Id == dd.ISMPRINC_Id) {
                                    cls.invoiceMode.push(dd);
                                }
                            });
                        });
                    }
                  
                }
                else {
                    swal("No records found")
                }
            });
        };

        // Updated export button it will print all records in a table 
        $scope.exportToExcel = function () {
            var printSectionId = '#printDeviation';
            var exportHref = Excel.tableToExcel(printSectionId, 'MANAGEMENT CLIENT REMINDER');
            $timeout(function () {
                var a = document.createElement('a');
                a.href = exportHref;
                a.download = "MANAGEMENT CLIENT REMINDER.xls";
                document.body.appendChild(a);
                a.click();
                a.remove();
            }, 100);

        };



        //===added by praveen
        $scope.all_check_Emp1 = function () {
            var toggleStatuss = $scope.obj.employeeck1;
            angular.forEach($scope.employeelistdata, function (itm) {
                itm.selected = toggleStatuss;
            });
        };

        $scope.isOptionsRequired1 = function () {
            if ($scope.setremind == false) {
                return !$scope.employeelistdata.some(function (options) {
                    return options.selected;
                });
            }

        };

        $scope.emptogchkbx1 = function () {
            $scope.employeeck = $scope.employeelistdata.every(function (types) {
                return types.userId;
            });
        };
        //Ended




        $scope.addgrnrows = function (data) {
            $scope.userlistdata = [];
            $scope.userlist = data.employeelistdata;


            if ($scope.multipleuserlevel != null && $scope.multipleuserlevel.length < 15) {
                for (var i = 0; i === $scope.multipleuserlevel.length; i++) {
                    var id = $scope.multipleuserlevel[i].itrS_Id;
                    var lastChar = id.substr(id.length - 1);
                    $scope.cnt = parseInt(lastChar);


                }

                $scope.cnt = $scope.cnt + 1;
                $scope.tet = 'trans' + $scope.cnt;
                var newItemNo = $scope.cnt;
                var ApprovalAlevel = $scope.multipleuserlevel.length + 1
                $scope.multipleuserlevel.push({ 'itrS_Id': 'trans' + newItemNo, hrplmgapP_ApprovalAlevel: ApprovalAlevel, employeelistdata: data.employeelistdata });
            }

        };
        $scope.removegrnrows = function (index, data) {
            //if ($scope.multipleuserlevel.length == index+1) {
            var newItemNo = $scope.multipleuserlevel.length - 1;
            $scope.multipleuserlevel.splice(index, 1);
            //}
            //else {
            //    swal("Please Delete Higher Level");
            //}

        };

        $scope.reminderdaysmincheck = function () {
            var maxdays = 0;
            angular.forEach($scope.multipleuserlevel, function (emp) {
                var temp = Number(emp.hrplmgapP_MaxApprovalDays || 0);
                maxdays = Number(maxdays || 0) + temp;
            });

            if (Number($scope.reminderdays < maxdays)) {
                swal("Total Reminder Maximum Days is Exceeding");
                angular.forEach($scope.multipleuserlevel, function (emp) {
                    emp.hrplmgapP_MaxApprovalDays = 0;

                });
            }
        }


        //========================================= Save
        $scope.obj = {};
        $scope.submitted = false;
        $scope.savedata = function () {
            
            var data = {};
            var varmeetingdate = $scope.hrplmG_MeetingDateTime == null ? "" : $filter('date')($scope.hrplmG_MeetingDateTime, "yyyy-MM-dd");
            var varentrydate = $scope.hrplmG_EntryDateTime == null ? "" : $filter('date')($scope.hrplmG_EntryDateTime, "yyyy-MM-dd");
            var ppdate = $scope.HRPLMG_PaidDate == null ? "" : $filter('date')($scope.HRPLMG_PaidDate, "yyyy-MM-dd");

            $scope.employeeArray = [];
            angular.forEach($scope.employeelist, function (emp) {
                if (data.userId)
                 $scope.employeeArray.push(emp.hrmE_Id);
            });
            if ($scope.ISMMCLT_Id.ISMMCLT_Id > 0) {
                $scope.ClientId = $scope.ISMMCLT_Id.ISMMCLT_Id;
            } else {
                $scope.ClientId=0
            } 
            

            $scope.submitted = true;
            if ($scope.myForm.$valid) {

                if ($scope.setremind == false) {
                    data = {
                        "HRPLMG_Id": $scope.HRPLMG_Id,
                    
                        "reminderflag": $scope.setremind,
                        "HRPLMG_PlannerName": $scope.hrplmG_PlannerName,
                        "HRPLMG_PlannerDetails": $scope.hrplmG_PlannerDetails,
                        "HRPLMG_MeetingPersonName": $scope.hrplmG_MeetingPersonName,
                        "HRPLMG_ContactNo": $scope.hrplmG_ContactNo,
                        "HRPLMG_MeetingAddress": $scope.hrplmG_MeetingAddress,
                        "HRPLMG_MeetingDateTime": varmeetingdate,
                        "HRPLMG_PaidDate": ppdate,
                        "HRPLMG_MeetingPurpose": $scope.hrplmG_MeetingPurpose,
                        "HRPLMG_PersonsAccompanying": $scope.hrplmG_PersonsAccompanying,
                        "HRPLMG_EntryDateTime": varentrydate,
                        "HRPLMG_Remarks": $scope.hrplmG_Remarks,
                        "HRPLMG_ReminderSchedule": $scope.hrplmG_ReminderSchedule,
                        "HRPLMG_ReminderFlag": $scope.hrplmG_ReminderFlag,
                        "HRPLMG_RepeatFlag": $scope.hrplmG_RepeatFlag,
                        "HRPLMG_MeetingStartTime": $filter('date')($scope.hrplmG_MeetingStartTime, "HH:mm"),
                        "HRPLMG_MeetingEndTime": $filter('date')($scope.hrplmG_MeetingEndTime, "HH:mm"),
                        emp_array: $scope.employeeArray,
                        filelist: $scope.materaldocuupload,
                    };
                }
                else {

                    var mobilesstd = [];
                    angular.forEach($scope.mobilesstd, function (mobile) {
                        if (mobile.hrmemnO_MobileNo != undefined && mobile.hrmemnO_MobileNo != "") {
                            mobilesstd.push(mobile);
                        }
                    });

                    var emailsstd = [];
                    angular.forEach($scope.emailsstd, function (email) {
                        if (email.hrmeM_EmailId != undefined && email.hrmeM_EmailId != "") {
                            emailsstd.push(email);
                        }
                    });
                    data = {
                      
                        "HRPLMG_Id": $scope.HRPLMG_Id,
                        "ISMMCLT_Id": $scope.ClientId,
                        "ISMPRINC_Id": $scope.ISMPRINC_Id,
                        "reminderflag": $scope.setremind,
                        "HRPLMG_PlannerName": $scope.hrplmG_PlannerName,
                        "HRPLMG_PlannerDetails": $scope.hrplmG_PlannerDetails,
                        "HRPLMG_MeetingDateTime": varmeetingdate,
                        "HRPLMG_EntryDateTime": varentrydate,
                        "HRPLMG_Remarks": $scope.hrplmG_Remarks,
                        "remiderday": $scope.reminderdays.toString(),
                        "HRPLMG_ReminderFlag": $scope.hrplmG_ReminderFlag,
                        "HRPLMG_RepeatFlag": $scope.hrplmG_RepeatFlag,
                        "HRPLMG_ReminderContent": $scope.HRPLMG_ReminderContent,
                        "HRPLMG_PaidDate": ppdate,
                        mobile_list_dto: mobilesstd,
                        email_list_dto: emailsstd,
                        emp_array: $scope.employeeArray,
                        "Multipleuserlvl": $scope.multipleuserlevel,
                    };
                }


                apiService.create("HR_Planner_Management/savedata", data).then(function (promise) {
                    if (promise.returnval === "Add") {
                        swal('Record Saved successfully');
                        $state.reload();
                        //$scope.clickreminder();
                    }
                    else if (promise.returnval === 'Update') {
                        swal('Record Updated successfully');
                        $state.reload();
                        //$scope.clickreminder();
                    }
                    else if (promise.returnval === 'Duplicate') {
                        swal('Record already exist');
                    }
                    else {
                        swal('Failed, please contact administrator');
                    }
                });
            }
            else {
                $scope.submitted = true;
            }
        };

        //---Deactivepage---//
        $scope.Deletedata = function (record, SweetAlert) {
            var pageid = record.hrplmG_Id;

            swal({
                title: "Are you sure?",
                text: "Do You Want To Delete Record?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, Delete it",
                cancelButtonText: "Cancel",
                closeOnConfirm: false,
                closeOnCancel: false
            },
                function (isConfirm) {
                    if (isConfirm) {
                        apiService.getURI("HR_Planner_Management/DeleteYY", pageid).
                            then(function (promise) {

                                if (promise.message != null) {
                                    swal(promise.message);
                                }
                                else {
                                    if (promise.resrnval == true) {
                                        swal("Record Delete Successfully.");

                                    }
                                    else {
                                        swal("Record Not Delete");

                                    }
                                }
                                $state.reload();

                            })
                    }
                    else {
                        swal("Record Deletion Cancelled");
                    }
                });
        }






        //--Active Deactive---//
        $scope.deactive = function (user, SweetAlert) {
            debugger
            var mgs = "";
            var confirmmgs = "";
            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }
            if (user.hrplmG_ActiveFlag == true) {
                mgs = "Deactive";
                confirmmgs = "Deactivated";

            }
            else {

                mgs = "Active";
                confirmmgs = "Activated";

            }
            swal({
                title: "Are you sure",
                text: "Do you want to " + mgs + " record??????",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, " + mgs + " it!",
                cancelButtonText: "Cancel",
                closeOnConfirm: false,
                closeOnCancel: false
            },
                function (isConfirm) {
                    if (isConfirm) {
                        apiService.create("HR_Planner_Management/activedeactive/", user).
                            then(function (promise) {
                                if (promise.message != null) {
                                    swal(promise.message);
                                }
                                else {
                                    if (promise.resrnval == true) {
                                        swal(confirmmgs + " " + "Successfully.");
                                        $scope.clickreminder();
                                    }
                                    else {
                                        swal(confirmmgs + " " + " Successfully");
                                        $scope.clickreminder();
                                    }
                                }
                            })
                    }
                    else {
                        swal("Record " + mgs + " Cancelled");
                    }
                    $scope.clickreminder();
                });
        }


        //=====================Editrecord....
        $scope.edit = function (task) {
            $scope.HRPLMG_Id = task.hrplmG_Id;
            var data = {
                "HRPLMG_Id": task.hrplmG_Id
            };
            apiService.create("HR_Planner_Management/edit", data).then(function (promise) {

                if (promise.editdata.length > 0) {
                    $scope.editflag = true;
                    $scope.hrplmG_PlannerName = promise.editdata[0].hrplmG_PlannerName;
                    $scope.hrplmG_PlannerDetails = promise.editdata[0].hrplmG_PlannerDetails;
                    $scope.hrplmG_MeetingPersonName = promise.editdata[0].hrplmG_MeetingPersonName;
                    $scope.hrplmG_ContactNo = promise.editdata[0].hrplmG_ContactNo;
                    $scope.hrplmG_MeetingAddress = promise.editdata[0].hrplmG_MeetingAddress;
                    if (promise.editdata[0].hrplmG_MeetingDateTime !== null) {
                        $scope.hrplmG_MeetingDateTime = new Date(promise.editdata[0].hrplmG_MeetingDateTime);
                    }
                    else {
                        $scope.hrplmG_MeetingDateTime = null;
                    }
                    if (promise.editdata[0].hrplmG_PaidDate !== null) {
                        $scope.HRPLMG_PaidDate = new Date(promise.editdata[0].hrplmG_PaidDate);
                    }
                    else {
                        $scope.HRPLMG_PaidDate = null;
                    }
                    //$scope.hrplmG_MeetingDateTime = promise.editdata[0].hrplmG_MeetingDateTime;
                    $scope.hrplmG_MeetingPurpose = promise.editdata[0].hrplmG_MeetingPurpose;
                    $scope.hrplmG_PersonsAccompanying = promise.editdata[0].hrplmG_PersonsAccompanying;
                    //$scope.hrplmG_EntryDateTime = promise.editdata[0].hrplmG_EntryDateTime;
                    if (promise.editdata[0].hrplmG_EntryDateTime !== null) {
                        $scope.hrplmG_EntryDateTime = new Date(promise.editdata[0].hrplmG_EntryDateTime);
                    }
                    else {
                        $scope.hrplmG_EntryDateTime = null;
                    }
                    $scope.hrplmG_Remarks = promise.editdata[0].hrplmG_Remarks;
                    $scope.hrplmG_ReminderSchedule = promise.editdata[0].hrplmG_ReminderSchedule;
                    $scope.hrplmG_ReminderFlag = promise.editdata[0].hrplmG_ReminderFlag;
                    $scope.hrplmG_RepeatFlag = promise.editdata[0].hrplmG_RepeatFlag;
                    $scope.hrmE_Id = promise.editdata[0].hrmE_Id;
                    if ($scope.setremind == false) {
                        $scope.hrplmG_MeetingStartTime = moment(promise.editdata[0].hrplmG_MeetingStartTime, 'HH:mm').format();
                        $scope.hrplmG_MeetingEndTime = moment(promise.editdata[0].hrplmG_MeetingEndTime, 'HH:mm').format();
                    }

                    $scope.materaldocuupload = [{ id: 'Materal1' }];
                    if (promise.editfiles != null && promise.editfiles.length > 0) {
                        $scope.materaldocuupload = promise.editfiles;
                        angular.forEach($scope.materaldocuupload, function (ddd) {

                            var img = ddd.cfilepath;
                            var imagarr = img.split('.');
                            var lastelement = imagarr[imagarr.length - 1];
                            ddd.filetype = lastelement;
                            if (lastelement === 'doc' || lastelement === 'docx' || lastelement === 'ppt' || lastelement === 'pptx' || lastelement === 'xlsx' || lastelement === 'xls') {
                                ddd.document_Pathnew = "https://view.officeapps.live.com/op/view.aspx?src=" + ddd.cfilepath;
                            }
                        })
                    }

                    //if (promise.editedlevelcheck != null && promise.editedlevelcheck.length > 0) {
                    //    $scope.multipleuserlevel = promise.editedlevelcheck;


                    //}


                    if (promise.editedlevelcheck != null && promise.editedlevelcheck.length > 0) {
                        $scope.multipleuserlevel = [{ id: 'Materal1' }];
                        $scope.multipleuserlevel = promise.editedlevelcheck;


                        angular.forEach($scope.multipleuserlevel, function (emp1) {

                            emp1.employeelistdata = $scope.employeelisttable;

                        })


                        //for (var i = 0; i <= promise.editedlevelcheck.length; i++) {
                        //    $scope.multipleuserlevel.push(
                        //        {
                        //            hrplmgapP_ApprovalAlevel: promise.editedlevelcheck[i].hrplmgapP_ApprovalAlevel,
                        //            hrplmgapP_MaxApprovalDays: promise.editedlevelcheck[i].hrplmgapP_MaxApprovalDays,
                        //            employeelistdata: $scope.employeelist
                        //        });


                        //}
                        //angular.forEach($scope.multipleuserlevel, function (emp) {
                        //    angular.forEach(promise.editedlevelcheck, function (emp1) {
                        //        if (emp.userId == emp1.userId) {
                        //            emp.employeelistdata.userId = emp1.userId;
                        //        }
                        //    })
                        //})

                    }


                    if ($scope.setremind == true) {

                        $scope.reminderdays = promise.editdata[0].hrplmG_ReminderSchedule;

                        if ($scope.reminderdays != null && $scope.reminderdays !== '') {
                            $scope.reminderdays = parseInt($scope.reminderdays);
                        }
                        $scope.HRPLMG_ReminderContent = promise.editdata[0].hrplmG_ReminderContent;
                        $scope.Performadetails = promise.performadetails;
                        $scope.Templatedetails = promise.templatedetails;
                        
                        $scope.ISMMCLT_Id = promise.editdata[0].ismmclT_Id;
                        $scope.ISMPRINC_Id = promise.editdata[0].ismprinC_Id;
                        
                        //$scope.ISMPRINC_Id = promise.editdata[0].ismprinC_Id;
                        for (var i = 0; i < $scope.Clientlist.length; i++) {
                            if ($scope.Clientlist[i].ISMMCLT_Id == $scope.ISMMCLT_Id) {            
                                $scope.ISMMCLT_Id = $scope.Clientlist[i];
                            }
                        }
                        $scope.OnchageInvoice();
                        $scope.moblist = promise.mobilenolist;
                        $scope.emblist = promise.emailist;

                        $scope.mobilesstd1 = [];    
                        angular.forEach($scope.moblist, function (ee) {
                            $scope.mobilesstd1.push({ hrmemnO_MobileNo: ee })

                        })
                        if ($scope.mobilesstd1.length > 0) {
                            $scope.mobilesstd = $scope.mobilesstd1;
                        }



                        $scope.emailsstd1 = [];
                        angular.forEach($scope.emblist, function (ee) {
                            $scope.emailsstd1.push({ hrmeM_EmailId: ee })

                        })
                        if ($scope.emailsstd1.length > 0) {
                            $scope.emailsstd = $scope.emailsstd1;
                        }
                    }


                }
            });
        };

        //=================Active/Deactivate
        $scope.deactiveqq = function (task) {
            $scope.ISMTCR_Id = task.ismtcR_Id;
            var dystring = "";
            if (task.ismtcR_ActiveFlg === true) {
                dystring = "Deactivate";
            }
            else if (task.ismtcR_ActiveFlg === false) {
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
                        apiService.create("HR_Planner_Management/deactive", task).
                            then(function (promise) {
                                if (promise.returnval === true) {
                                    swal("Record " + dystring + "d Successfully!!!");
                                }
                                else {
                                    swal("Record Not " + dystring + "d Successfully!!!");
                                }
                                $state.reload();
                            });
                    }
                    else {
                        swal("Record  " + dystring + " Cancelled!!!");
                    }
                });
        };

        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        //===========Clear Field
        $scope.cancel = function () {
            $state.reload();
        };

        $scope.all_check_Emp = function () {
            var toggleStatuss = $scope.employeeck;
            angular.forEach($scope.employeelist, function (itm) {
                itm.selected = toggleStatuss;
            });
        };

        $scope.isOptionsRequired = function () {
            if ($scope.setremind == false) {
                return !$scope.employeelist.some(function (options) {
                    return options.selected;
                });
            }

        };

        $scope.emptogchkbx = function () {
            $scope.employeeck = $scope.employeelist.every(function (types) {
                return types.hrmEId;
            });
        };




        $scope.removeNewMobile1std = function (index, curval1std) {

            var newItemNostd2 = $scope.mobilesstd.length - 1;
            if (newItemNostd2 !== 0) {
                $scope.delmsrd = $scope.mobilesstd.splice(index, 1);
            }


        };
        $scope.showAddEmail1std = function (email) {
            return email.id === $scope.emailsstd[$scope.emailsstd.length - 1].id;
        };
        $scope.emailsstd = [{ id: 'emailsstd1' }];
        $scope.addNewEmail1std = function () {
            var newItemNostd2 = $scope.emailsstd.length + 1;
            if (newItemNostd2 <= 100) {
                $scope.emailsstd.push({ 'id': 'emailsstd' + newItemNostd2 });
            }
        };
        $scope.mobilesstd = [{ id: 'mobilestd1' }];
        $scope.addNewMobile1std = function () {
            var newItemNostd = $scope.mobilesstd.length + 1;
            if (newItemNostd <= 100) {
                $scope.mobilesstd.push({ 'id': 'mobilestd' + newItemNostd });
            }
            if (newItemNostd == 100) {
                $scope.myForm1.$setPristine();
            }
        };


        $scope.removeNewEmail1std = function (index, id) {
            var newItemNostd = $scope.emailsstd.length - 1;
            if (newItemNostd !== 0) {
                $scope.delmsrd = $scope.emailsstd.splice(index, 1);
            }
        };



        $scope.materaldocuupload = [{ id: 'Materal1' }];
        $scope.addmateral = function () {
            var newItemNo = $scope.materaldocuupload.length + 1;
            if (newItemNo <= 100) {
                $scope.materaldocuupload.push({ 'id': 'Materal' + newItemNo });
            }
        };

        $scope.removemateral = function (index) {
            var newItemNo = $scope.materaldocuupload.length - 1;
            $scope.materaldocuupload.splice(index, 1);
            if ($scope.materaldocuupload.length === 0) {
                //data
            }
        };

        // Save Function For Materal Guide Upload

        $scope.uploadmateraldocuments1 = [];

        $scope.uploadmateraldocuments = function (input, document) {

            $scope.uploadmateraldocuments1 = input.files;

            $scope.filename = input.files[0].name;

            if (input.files && input.files[0]) {

                if ((input.files[0].type === "image/jpeg" || input.files[0].type === "image/png" || input.files[0].type === "image/jpg")
                    && input.files[0].size <= 2097152)  // 2097152 bytes = 2MB 
                {
                    UploaddianmateralPhoto(document);
                }
                else if (input.files[0].type === "video/mp4" && input.files[0].size <= 2097152) {
                    UploaddianmateralPhoto(document);
                }
                else if (input.files[0].type === "application/pdf" && input.files[0].size <= 2097152) {
                    UploaddianmateralPhoto(document);
                }
                else if (input.files[0].type === "application/msword" && input.files[0].size <= 2097152) {
                    UploaddianmateralPhoto(document);
                }
                else if (input.files[0].type === "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" && input.files[0].size <= 2097152) {
                    UploaddianmateralPhoto(document);
                }
                else if (input.files[0].type === "application/vnd.ms-excel") {
                    UploaddianmateralPhoto(document);
                }
                else if (input.files[0].type === "application/vnd.openxmlformats-officedocument.wordprocessingml.document" && input.files[0].size <= 2097152) {
                    UploaddianmateralPhoto(document);
                }
                else if (input.files[0].size > 2097152) {
                    swal("Image size should be less than 2MB");
                    return;
                } else {
                    swal("Upload MP4, Pdf, Image Files Only");
                }
            }
        };

        function UploaddianmateralPhoto(data) {
            console.log("Student Upload  :" + data);
            var formData = new FormData();
            for (var i = 0; i <= $scope.uploadmateraldocuments1.length; i++) {
                formData.append("File", $scope.uploadmateraldocuments1[i]);
            }
            var defer = $q.defer();
            $http.post("/api/ImageUpload/Uploadtrnsportdocuments", formData,
                {
                    withCredentials: true,
                    headers: { 'Content-Type': undefined },
                    transformRequest: angular.identity
                })
                .success(function (d) {
                    defer.resolve(d);
                    data.cfilepath = d;
                    data.cfilename = $scope.filename;
                    $('#').attr('src', data.cfilepath);
                    var img = data.cfilepath;
                    var imagarr = img.split('.');
                    var lastelement = imagarr[imagarr.length - 1];
                    data.filetype = lastelement;
                    if (lastelement === 'doc' || lastelement === 'docx' || lastelement === 'ppt' || lastelement === 'pptx' || lastelement === 'xlsx' || lastelement === 'xls') {
                        data.document_Pathnew = "https://view.officeapps.live.com/op/view.aspx?src=" + data.cfilepath;
                    }
                })
                .error(function () {
                    defer.reject("File Upload Failed!");
                });
        }

        $scope.trustSrc = function (src) {
            return $sce.trustAsResourceUrl(src);
        };


        $scope.showmothersign = function (path) {
            $('#preview1').attr('src', path);
            $('img').bind('contextmenu', function (e) {
                return false;
            });
            $('#myModalimg').modal('show');
        };

        $scope.showGuardianPhotonew = function (data) {
            $scope.view_videos = [];
            $scope.videoSources = [];
            $scope.preview1 = data.cfilepath;
            $scope.videdfd = data.cfilepath;
            $scope.movie = { src: data.cfilepath };
            $scope.movie1 = { src: data.cfilepath };
            $scope.view_videos.push({ id: 1, coeeV_Videos: data.cfilepath });
            console.log($scope.view_videos);
        };

        $scope.showpdf = false;
        $scope.downloadview = function (pdfview) {
            $scope.pdfurl = pdfview;
            $scope.showpdf = true;
            $('#showpdf').modal('show');
        };

        $scope.backtoview = function () {
            $scope.showpdf = false;
        };


        $scope.objdata = [];
        $scope.viewdocument = function (obj) {
            $scope.uploaddocfiles = [];
            $scope.uploadfilesdetails = [];
            $scope.objdata = obj;

            apiService.create("HR_Planner_Management/viewuploadflies", obj).then(function (promise) {
                if (promise !== null) {

                    $scope.uploadfilesdetails = promise.editfiles;
                    if (promise.editfiles !== null && promise.editfiles.length > 0) {
                        $scope.uploaddocfiles = promise.editfiles;

                        angular.forEach($scope.uploaddocfiles, function (dd) {
                            var img = dd.cfilepath;
                            var imagarr = img.split('.');
                            var lastelement = imagarr[imagarr.length - 1];
                            dd.filetype = lastelement;
                            if (lastelement === 'doc' || lastelement === 'docx' || lastelement === 'ppt' || lastelement === 'pptx' || lastelement === 'xlsx' || lastelement === 'xls') {
                                dd.document_Pathnew = "https://view.officeapps.live.com/op/view.aspx?src=" + dd.cfilepath;
                            }
                        });
                    } else {
                        $('#popup11').modal('hide');
                        swal("No Documents Found");
                    }
                }
            });
        };


        $scope.onview = function (filepath, filename) {

            var imagedownload = filepath;
            $scope.content = "";
            var fileURL = "";
            var file = "";
            $http.get(imagedownload, { responseType: 'arraybuffer' })
                .success(function (response) {
                    file = new Blob([(response)], { type: 'application/pdf' });
                    fileURL = URL.createObjectURL(file);
                    //  $scope.content = $sce.trustAsResourceUrl(fileURL);
                    var pdfId = document.getElementById("pdfIdzz");
                    pdfId.removeChild(pdfId.childNodes[0]);
                    var embed = document.createElement('embed');
                    embed.setAttribute('src', fileURL);
                    embed.setAttribute('type', 'application/pdf');
                    embed.setAttribute('width', '100%');
                    embed.setAttribute('height', '1000');
                    pdfId.appendChild(embed);
                    $('#showpdf').modal('show');
                });
        };




        $scope.getmailid = function (obj, stu) {
            var data = {
                "UserId": obj
            };
            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }
            apiService.create("HR_Planner_Management/getempmailid", data).
                then(function (promise) {

                    stu.employeeEmail = promise.getempmailid[0].hrmeM_EmailId;

                })
        };



        $scope.deleteuploadfile = function (obj) {

            var data = {
                "HRPLMGFL_Id": obj.cfileid
            };

            swal({
                title: "Are You Sure",
                text: "Do You Want To Delete The Record ?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, Delete It!",
                cancelButtonText: "Cancel",
                closeOnConfirm: false,
                closeOnCancel: false
            },
                function (isConfirm) {
                    if (isConfirm) {
                        apiService.create("HR_Planner_Management/deleteuploadfile", data).then(function (promise) {
                            //if (promise.already_cnt === true) {
                            //    swal("You Can Not Deactivate This Record,It Has Dependency");
                            //}
                            //else {
                            if (promise.resrnval === true) {
                                swal("Record Deleted successfully");
                            }
                            else {
                                swal("Record Deletion Failed");
                            }
                            //}
                            //$('#popup11').modal('hide');

                            $scope.viewdocument($scope.objdata)
                        });
                    }
                    else {
                        swal("Record Deletion Cancelled");
                    }
                });
        };



        //////end////

    }



    angular.module('app').filter("trustUrl", function ($sce) {
        return function (Url) { return $sce.trustAsResourceUrl(Url); };
    });

    angular.module('app').directive('txtArea', function () {
        return {
            restrict: 'AE',
            replace: 'true',
            scope: { data: '=', model: '=ngModel' },
            template: "<textarea></textarea>",
            link: function (scope, elem) {
                scope.$watch('data', function (newVal) {
                    if (newVal) {
                        scope.model += newVal[0];
                    }
                });
            }
        };
    });

})();   
