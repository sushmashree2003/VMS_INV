(function () {
    'use strict';
    angular.module('app').controller('EmployeeRatingReportController', EmployeeRatingReportController)

    EmployeeRatingReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache', '$filter', '$timeout', 'Excel']
    function EmployeeRatingReportController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache, $filter, $timeout, Excel) {

        $scope.submitted = false;
        $scope.indentapproveddetais = [];
        $scope.maxdate = new Date();

        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        if (admfigsettings !== null && admfigsettings.length > 0) {
            var logopath = admfigsettings[0].asC_Logo_Path;
        }
        var temp = [];
        var year = "";
        $scope.yearfromdate = "";
        $scope.monthlist_temp = [];
        $scope.consolidata_id = [];
        $scope.get_deviationreport = [];
        $scope.imgname = logopath;

        $scope.Loaddata = function () {
            $scope.currentPage = 1;
            $scope.itemsPerPage = 10;
            $scope.search = "";

            var pageid = 2;
            apiService.getURI("Rating_Transaction/get_Consolidated_details", pageid).then(function (promise) {
                // $scope.get_emp_list = promise.get_emp_list;
                $scope.monthlist_temp = promise.month_array;
                $scope.arrlist8 = promise.yearlist;
                $scope.get_userEmplist = promise.get_userEmplist;
                $scope.departmentdropdown = promise.departmentList;
                $scope.designationdropdown = promise.designation;
                $scope.change_year();
            });
        };

        $scope.OnChangeExpenditureInstitution = function () {
            $scope.indentapproveddetais = [];
            $scope.getindenapprovedreportdetails = [];
        };

        $scope.onchangefromdate = function () {
            $scope.PCINDENT_Date_To = null;
            $scope.indentapproveddetais = [];
            $scope.getindenapprovedreportdetails = [];
        };

        $scope.onchangedate = function () {
            $scope.indentapproveddetais = [];
            $scope.getindenapprovedreportdetails = [];
        };
        $scope.obj.searchValueDeptd = '';
        $scope.filterValue4 = function (obj) {
            return angular.lowercase(obj.HRMDC_Name).indexOf(angular.lowercase($scope.obj.searchValueDeptd)) >= 0;
        };

        $scope.obj.searchValueDesgd = '';
        $scope.filterValue = function (obj) {
            return angular.lowercase(obj.HRMDES_DesignationName).indexOf(angular.lowercase($scope.obj.searchValueDesgd)) >= 0 || angular.lowercase(obj.MI_Name).indexOf(angular.lowercase($scope.obj.searchValueDesgd)) >= 0;
        };

        $scope.obj.searchValueUEMd = '';
        $scope.filterValue2 = function (obj) {
            return angular.lowercase(obj.userEmpName).indexOf(angular.lowercase($scope.obj.searchValueUEMd)) >= 0 || angular.lowercase(obj.HRMD_DepartmentName).indexOf(angular.lowercase($scope.obj.searchValueUEMd)) >= 0;
        };
        $scope.getreport = function (PCINDENT_Idss) {
            $scope.submitted = true;
            if ($scope.myForm.$valid) {

                $scope.month_list = [];
              
                angular.forEach($scope.filterValue8, function (em) {
                    if (em.selected === true) {
                        $scope.month_list.push(em);
                    }
                });

              
                var data = {

                    "year": $scope.fmtP_Years,
                    "monthList": $scope.month_list,
                    "flag": "monthyearwise"
                };
                apiService.create("Rating_Transaction/getemployeereport", data).then(function (promise) {
                    if (promise !== null) {

                        $scope.consolidateData = promise.consolidateData;

                        $scope.get_deviationreport = promise.consolidateData;
                        //  if ($scope.get_deviationreport !== null && $scope.get_deviationreport.length > 0) {

                        //  $scope.consolidateData = $scope.consolidateData;
                        if ($scope.get_deviationreport !== null && $scope.get_deviationreport.length > 0) {

                            //  $scope.consolidata_id = promise.consolidateData_ids;
                            //$scope.get_deviationreport = promise.consolidateData;
                            //angular.forEach($scope.consolidata_id, function (dd) {
                            //    angular.forEach($scope.get_deviationreport, function (pp) {

                            //    if (pp.hrme_id == dd.HRME_Id) {
                            //        //$scope.consolidata_idnew.push({ January: pp.January, February: pp.February, March: pp.March, April: pp.April, May: pp.May, June: pp.June, July: pp.July, August: pp.August, September: pp.September, October: pp.October, November: pp.November, December: pp.December, RATMQE_RatingQuestions: pp.RATMQE_RatingQuestions, TotalRating: pp.TotalRating});
                            //        $scope.consolidata_idnew.push(dd);
                            //    }
                            //    });
                            //});
                            $scope.employee = $scope.get_deviationreport[0].EmpName;

                            $scope.employeeid = [];
                            $scope.employeename = [];
                            $scope.employeenamee = [];

                            angular.forEach($scope.consolidateData, function (de) {

                                if ($scope.employeename.length === 0) {
                                    $scope.employeename.push({ EmpName: de.EmpName, HRME_Id: de.HRME_Id });



                                } else if ($scope.employeename.length > 0) {
                                    var intcount = 0;
                                    angular.forEach($scope.employeename, function (em) {
                                        if (em.HRME_Id === de.HRME_Id) {
                                            intcount += 1;
                                        }
                                    });
                                    if (intcount === 0) {
                                        $scope.employeename.push({ EmpName: de.EmpName, HRME_Id: de.HRME_Id });

                                    }
                                }


                            });
                            var sum_avg = 0;
                            var avg_qn = 0;
                            var qn_cnt = 0;
                            angular.forEach($scope.get_deviationreport, function (dev) {
                                if ($scope.employeeid.length === 0) {
                                    $scope.employeeid.push({ HRME_Id: dev.HRME_Id, RATMQE_RatingQuestions: dev.RATMQE_RatingQuestions, RATMT_Id: dev.RATMT_Id, RATMQE_Id: dev.RATMQE_Id, EmpName: dev.EmpName, RATMT_RatingTypeName: dev.RATMT_RatingTypeName, TotalAverage: dev.TotalAverage });



                                }
                                else if ($scope.employeeid.length > 0) {
                                    var intcount = 0;
                                    angular.forEach($scope.employeeid, function (emp) {
                                        if (emp.HRME_Id === dev.HRME_Id) {
                                            intcount += 1;
                                        }
                                    });
                                    if (intcount === 0) {
                                        $scope.employeeid.push({
                                            HRME_Id: dev.HRME_Id, RATMQE_RatingQuestions: dev.RATMQE_RatingQuestions, RATMT_Id: dev.RATMT_Id, RATMQE_Id: dev.RATMQE_Id, EmpName: dev.EmpName, RATMT_RatingTypeName: dev.RATMT_RatingTypeName, TotalAverage: dev.TotalAverage
                                        });

                                    }
                                }


                            });

                            // var overall_avg = 0;


                            angular.forEach($scope.employeeid, function (ddd) {
                                $scope.templist = [];
                                var overall_avg = 0;
                                angular.forEach($scope.get_deviationreport, function (dd) {
                                    if (dd.HRME_Id === ddd.HRME_Id) {

                                        if (dd.January != null && dd.January >= -2) {
                                            qn_cnt += 1
                                        }
                                        if (dd.February != null && dd.February >= -2) {
                                            qn_cnt += 1
                                        }
                                        if (dd.March != null && dd.March >= -2) {
                                            qn_cnt += 1
                                        }
                                        if (dd.April != null && dd.April >= -2) {
                                            qn_cnt += 1
                                        }
                                        if (dd.May != null && dd.May >= -2) {
                                            qn_cnt += 1
                                        }
                                        if (dd.June != null && dd.June >= -2 ) {
                                            qn_cnt += 1
                                        }
                                        if (dd.July != null && dd.July >= -2) {
                                            qn_cnt += 1
                                        }
                                        if (dd.August != null && dd.August >= -2) {
                                            qn_cnt += 1
                                        }
                                        if (dd.September != null && dd.September >= -2) {
                                            qn_cnt += 1
                                        }
                                        if (dd.October != null && dd.October >= -2) {
                                            qn_cnt += 1
                                        }
                                        if (dd.November != null && dd.November >= -2) {
                                            qn_cnt += 1
                                        }
                                        if (dd.December != null && dd.December >= -2) {
                                            qn_cnt += 1;
                                        }
                                        //  var rr = dd.TotalRating / qn_cnt;
                                        var rr = (dd.TotalRating / qn_cnt);




                                        if (rr >= -2) {
                                            dd.TotalRating = rr.toFixed(2);;
                                        }
                                        else {
                                            dd.TotalRating = 0;

                                        }
                                        overall_avg += Number(dd.TotalRating);
                                        qn_cnt = 0;

                                        $scope.templist.push(dd);


                                    }

                                });
                                ddd.plannerdetails = $scope.templist;
                                ddd.TotalAverage = (overall_avg / $scope.templist.length).toFixed(2);


                            });

                            console.log($scope.employeeid[0].plannerdetails[0]);
                            console.log($scope.month_list);
                        }
                        if (promise.returnval == false) {
                            swal('No Record Found');

                        }
                    }
                });
            }
            else {
                $scope.submitted = true;
            }
        };

        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        $scope.headinstallmentid = function (month_id) {

            var data = {
                "year": $scope.fmtP_Years,
                "monthid": month_id,
               
            };

            apiService.create("Rating_Transaction/subclassificationwisereport/", data).
                then(function (promise) {

                    $scope.subclassification = [];

                    $scope.subclassificationreport = promise.consolidateData;

                    if ($scope.subclassificationreport.length > 0) {
                        $scope.PCMonthName  = $scope.subclassificationreport[0].PCMonthName;
                    }

                    angular.forEach($scope.subclassificationreport, function (de) {

                        if ($scope.subclassification.length === 0) {
                            $scope.subclassification.push({ RATMQECLSUB_Id: de.RATMQECLSUB_Id, QuestionsSubclassification: de.RATMQECLSUB_QuestionsSubclassification });

                        } else if ($scope.subclassification.length > 0) {
                            var intcount = 0;
                            angular.forEach($scope.subclassification, function (em) {
                                if (em.RATMQECLSUB_Id === de.RATMQECLSUB_Id) {
                                    intcount += 1;
                                }
                            });
                            if (intcount === 0) {
                                $scope.subclassification.push({ RATMQECLSUB_Id: de.RATMQECLSUB_Id, QuestionsSubclassification: de.RATMQECLSUB_QuestionsSubclassification });

                            }
                        }
                    });

                    angular.forEach($scope.subclassification, function (ddd) {
                        $scope.templist = [];
                        angular.forEach($scope.subclassificationreport, function (dd) {
                            if (dd.RATMQECLSUB_Id === ddd.RATMQECLSUB_Id) {
                                $scope.templist.push({ RATMQE_Id: dd.RATMQE_Id, RATMQE_RatingQuestions: dd.RATMQE_RatingQuestions, RATMOP_OptionsValue: dd.RATMOP_OptionsValue})
                            }

                        })
                        ddd.ratingquestion = $scope.templist;
                    })
                })
        }


        $scope.exportToExceldetails = function () {

            var excelname = 'Consolidated Details Report';
            excelname = excelname.toUpperCase() + '.xls';
            var exportHref = Excel.tableToExcel('#printSectionId', 'Consolidated Details Report');
            $timeout(function () {
                var a = document.createElement('a');
                a.href = exportHref;
                a.download = excelname;
                document.body.appendChild(a);
                a.click();
                a.remove();
            }, 100);
        };

        $scope.Print = function () {
            var innerContents = document.getElementById("printSectionId").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            //popupWinindow.document.write('<html><head>' +
            //    '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
            //    '<link type="text/css" media="print" rel="stylesheet" href="css/print/printpdf.css" />' +
            //    '<link type="text/css" media="print" href="css/print/preadmission/AdmissionReports.css" rel="stylesheet" />' +
            //    '</head> <body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>'
            //);

            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/printpdf.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/Fees/MonthEndReportPdf.css" />' +
                '<link type="text/css" media="print" href="css/print/PrintPdf_Bootstrap.css" rel="stylesheet" />' +
                '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');

            popupWinindow.document.close();
        };

        $scope.Clearid = function () {
            $state.reload();
        };


        //added by roopa
        //*************************** Department REPORT *********************//   
        //month selection
        $scope.all_checkm = function (checkallm) {
            var toggleStatus = checkallm;
            angular.forEach($scope.filterValue8, function (itm) {
                itm.selected = toggleStatus;
            });
            $scope.searchValueDesg = '';

            //  $scope.get_month();

        };

        $scope.change_year = function () {
            $scope.fillyear = [];
            angular.forEach($scope.arrlist8, function (dd) {
                if (dd.asmaY_Id === parseInt($scope.asmaY_Id)) {
                    var yearname = dd.asmaY_Year;
                    var yearname1 = yearname.split('-');
                    $scope.fillyear.push({ fmtP_Year: yearname1[0] });
                    $scope.fillyear.push({ fmtP_Year: yearname1[1] });
                }
            });
        }
        $scope.togchkbxm = function (groupType) {

            $scope.checkallm = $scope.filterValue8.every(function (itm) {
                return itm.selected;
            });
            $scope.searchValueDesg == '';
            //$scope.designationselectedAll = false;
            //$scope.Deptselectiondetails();
            //$scope.DeptdeviationRemarksReport = [];
            //$scope.searchValueUEM = '';
            //$scope.employeeid = [];
            //$scope.checkall = false;
            //$scope.get_deviationreport = [];
        };
        //
        //$scope.all_checkdept = function (departmentselectedAll) {

            

        //    var toggleStatus = departmentselectedAll;
        //    if (toggleStatus == false) {

        //        angular.forEach($scope.filterValue5, function (itm) {

        //            itm.selected = false;
        //        });

        //    }
        //    else {
        //        angular.forEach($scope.filterValue5, function (itm) {

        //            itm.selected = toggleStatus;
        //        });
        //    }

        //    $scope.searchValueDesg = '';
        //    $scope.designationselectedAll = false;
            
        //    $scope.DeptdeviationRemarksReport = [];
        //    $scope.employeeid = [];
        //    $scope.get_deviationreport = [];
        //};
        //$scope.togchkbxdept = function (groupType) {

           
        //    if (groupType.selected == false) {
                
        //        $scope.departmentselectedAll = $scope.departmentdropdown.every(function (itm) {
        //            return itm.selected = false;
        //        });
        //    }
        //    else {
        //        $scope.departmentselectedAll = $scope.departmentdropdown.every(function (itm) {
        //            return itm.selected;
        //        });

        //    }

        //    $scope.searchValueDesg == '';
        //    $scope.designationselectedAll = false;
           
        //    $scope.DeptdeviationRemarksReport = [];
        //    $scope.searchValueUEM = '';
        //    $scope.employeeid = [];
        //    $scope.checkall = false;
        //    $scope.get_deviationreport = [];
        //};


        //*************************** Designation REPORT *********************//        
        //$scope.all_checkdesg = function (designationselectedAll) {
          
        //    var toggleStatus = designationselectedAll;
        //    if (toggleStatus == false) {

        //        angular.forEach($scope.filterValue1, function (itm) {

        //            itm.selected = false;
        //        });

        //    }
        //    else {
        //        angular.forEach($scope.filterValue1, function (itm) {

        //            itm.selected = toggleStatus;
        //        });
        //    }


        //    $scope.searchValueUEM = '';
        //    $scope.checkall = false;
          
        //    $scope.DeptdeviationRemarksReport = [];
        //    $scope.employeeid = [];
        //    $scope.get_deviationreport = [];
        //};
        //$scope.togchkbxdesg = function (groupType) {
          

        //    if (groupType.selected == false) {
        //        $scope.filterValue1 = [];
        //        $scope.designationdropdown = [];
        //    }
        //    else {
        //        $scope.designationselectedAll = $scope.designationdropdown.every(function (itm) {
        //            return itm.selected;
        //        });

        //    }


  
        //    $scope.searchValueUEM = '';
        //    $scope.checkall = false;
            
        //    $scope.DeptdeviationRemarksReport = [];
        //    $scope.employeeid = [];
        //    $scope.get_deviationreport = [];
        //};

        $scope.all_check = function (ack) {
            $scope.checkall = ack;
            var toggleStatus = $scope.checkall;
            angular.forEach($scope.filterValue3, function (uem) {
                uem.selected = toggleStatus;
            });
            $scope.DeptdeviationRemarksReport = [];
            $scope.employeeid = [];
            $scope.get_deviationreport = [];
        };
        $scope.togchkbx = function () {
            $scope.checkall = $scope.get_userEmplist.every(function (options) {
                return options.selected;
            });
            $scope.DeptdeviationRemarksReport = [];
            $scope.employeeid = [];
            $scope.get_deviationreport = [];
        };

        $scope.get_years = function () {
            $scope.month_name = [];
            $scope.monthmodel = "";
            $scope.yearmodel = "";
            $scope.monthiddd = "";

            temp = [];
            angular.forEach($scope.yearlist, function (itm) {
                if (parseInt(itm.asmaY_Id) === parseInt($scope.academicyr)) {
                    year = itm.asmaY_Year;
                }
            });
            var s1 = year.substring(0, 4);
            var s2 = year.substring(year.length, 5);
            temp.push({ asmaY_Id: 0, asmaY_Year: s1 });
            temp.push({ asmaY_Id: 1, asmaY_Year: s2 });

            angular.forEach($scope.yearlist, function (itm) {
                if (parseInt(itm.asmaY_Id) === parseInt($scope.academicyr)) {
                    $scope.yearfromdate = itm.asmaY_From_Date;
                }
            });


            $scope.asmaYFromDate = $scope.yearfromdate;

            var date = new Date($scope.asmaYFromDate);

            $scope.monthNames = ["January", "February", "March", "April", "May", "June",
                "July", "August", "September", "October", "November", "December"
            ];
            $scope.months = [];
            $scope.year = [];

            for (var i1 = 0; i1 < 12; i1++) {
                $scope.months.push($scope.monthNames[date.getMonth()]);
                date.setMonth(date.getMonth() + 1);
            }

            $scope.monthByOrder = [];
            for (var i = 0; i < $scope.months.length; i++) {
                name = $scope.months[i].trim();
                for (var j = 0; j < $scope.monthlist_temp.length; j++) {
                    var monthiddd = $scope.monthlist_temp[j].ivrM_Month_Id;
                    if (name.toLowerCase() === $scope.monthlist_temp[j].ivrM_Month_Name.toLowerCase().trim()) {
                        if (i === 0) {
                            $scope.monthiddd = $scope.monthlist_temp[j].ivrM_Month_Id;
                        }
                        $scope.monthByOrder.push($scope.monthlist_temp[j]);
                    }
                }
            }
            $scope.monthList = $scope.monthByOrder;
            $scope.month_name = $scope.monthByOrder;



        };

        $scope.isOptionsRequired = function () {
            return !$scope.monthlist_temp.some(function (options) {
                return options.selected;
            });
        };
        $scope.isOptionsRequired1 = function () {
            return !$scope.departmentdropdown.some(function (options) {
                return options.selected;
            });
        };
        $scope.isOptionsRequired2 = function () {
            return !$scope.designationdropdown.some(function (options) {
                return options.selected;
            });
        };


        //$scope.get_month = function () {
        //    temp = [];
        //    angular.forEach($scope.yearlist, function (mm) {
        //        if (parseInt(mm.asmaY_Id) === parseInt($scope.academicyr)) {

        //            var frommonth = (new Date(mm.asmaY_From_Date)).getMonth() + 1;
        //            var tomonth = (new Date(mm.asmaY_To_Date)).getMonth() + 1;
        //            var fromyear = (new Date(mm.asmaY_From_Date)).getFullYear();
        //            var tomonthyear = (new Date(mm.asmaY_To_Date)).getFullYear();
        //            var s1 = "";
        //            var s2 = "";
        //            if ($scope.monthmodel >= $scope.monthiddd && $scope.monthmodel <= 12) {
        //                var year1 = mm.asmaY_Year;
        //                s1 = year1.substring(0, 4);
        //                s2 = year1.substring(year1.length, 5);
        //                temp.push({ asmaY_Id: 0, asmaY_Year: s1 });
        //                $scope.years = temp;

        //            } else {
        //                var year12 = mm.asmaY_Year;
        //                s1 = year12.substring(0, 4);
        //                s2 = year12.substring(year12.length, 5);
        //                temp.push({ asmaY_Id: 1, asmaY_Year: s2 });
        //                $scope.years = temp;
        //            }
        //        }
        //    });
        //};
    }
})();
