(function () {
    'use strict';
    angular.module('app').controller('BudgetApprovalController', BudgetApprovalController)
    BudgetApprovalController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache', '$filter', '$q', 'Excel', '$timeout']
    function BudgetApprovalController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache, $filter, $q, Excel, $timeout) {
        //$scope.search1235 = "";
        $scope.sort = function (keyname) {
            $scope.sortKey = keyname;   //set the sortKey to the param passed
            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
        };

        $scope.obj = {};
        $scope.submitted = false;

        var paginationformasters;
        var copty;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings !== null) {
            if (ivrmcofigsettings.length > 0) {
                paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
                copty = ivrmcofigsettings[0].ivrmgC_Disclaimer;
            } else {
                paginationformasters = 10;
            }
        } else {
            paginationformasters = 10;
        }
        var configsettingsnew = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        $scope.itemsPerPage = paginationformasters;

        $scope.currentPage = 5;
        $scope.search1235 = "";

        //-----------Tax Grid Select All
        $scope.allmi = false;



        $scope.loaddata = function () {
            $scope.obj.MI_Id = "";
            var pageid = 2;
            $scope.Month_List = [];
            $scope.deptList = [];

            apiService.getURI("Vms_Budget/BudgetHeadsReport1", pageid)
                .then(function (promise) {
                    $scope.allcompany = promise.allcompany;
                    $scope.FinancialYear = promise.financialYear;
                    $scope.Month_List = promise.month_List;

                    $scope.datagraph = [];
                    if ($scope.Month_List && $scope.Month_List.length > 0) {
                        for (var i = 0; i < $scope.Month_List.length; i++) {
                            $scope.datagraph.push({ label: $scope.Month_List[0].ivrM_Month_Name })
                        }
                    }
                });
        };
      
        $scope.onTotalAmountChanged = function (user) {
            if (!user.TotalAmount || user.TotalAmount < 0) return;

            var monthlyAmount = +(user.TotalAmount / 12).toFixed(2); // even split, round to 2 decimals if needed

            user.April = monthlyAmount;
            user.May = monthlyAmount;
            user.June = monthlyAmount;
            user.July = monthlyAmount;
            user.August = monthlyAmount;
            user.September = monthlyAmount;
            user.October = monthlyAmount;
            user.November = monthlyAmount;
            user.December = monthlyAmount;
            user.January = monthlyAmount;
            user.February = monthlyAmount;
            user.March = monthlyAmount;
        };

     
        $scope.Approve = function (data) {
            $scope.tempselected = [];  

            //angular.forEach($scope.templist, function (itm) {
            //    if (itm.selected === true) {


            //        $scope.tempselected.push(itm);  
            //    }
            //});

            //angular.forEach($scope.templist, function (itm) {
            //    if (itm.selected === true) {
            //        $scope.tempselected.push(itm)
            //    }
            //});

          

            angular.forEach($scope.reportBudgetlist, function (dept) {
                angular.forEach(dept.templist, function (itm) {
                    if (itm.selected === true) {
                        $scope.tempselected.push({ HRMD_Id: itm.HRMD_Id, VMBDGHDAMTN_Id: itm.VMBDGHDAMTN_Id, VMBDGHDN_Id: itm.VMBDGHDN_Id, TotalAmount: itm.TotalAmount });
                    }
                });
            });


            //angular.forEach($scope.templist, function (itm) {
            //    if (itm.selected === true) {
            //        $scope.tempselected.push({ VMBDGHDAMTN_Id: itm.VMBDGHDAMTN_Id, HRMD_Id: itm.HRMD_Id,  })
            //    }
            //});


            if ($scope.tempselected.length > 0) {

                var data = {
                    "Flag": data,
                    "saveremrksdata": $scope.tempselected ,
                    
                    //"HRMD_Id": $scope.tempselected[0].HRMD_Id,
                    //"VMBDGHDN_Id": $scope.tempselected[0].VMBDGHDN_Id,
                    //"VMBDGHDAMTN_Id": $scope.tempselected[0].VMBDGHDAMTN_Id,
                    //"TotalAmount":$scope.tempselected[0].TotalAmount
                };

            
                apiService.create("Vms_Budget/BudgetApprovalUpdate", data).then(function (promise) {
           
                    if (promise.returnval === "Save") {
                        swal("Amount Updated successfully");
                        $state.reload();
                        $scope.loaddata();
                    } else if (promise.returnval === "NotSave") {
                        swal("Amount Not Updated..");
                    } else {
                        swal("Please contact Administrator..");
                    }
                });
            } else {
                // If no items are selected, show a warning
                swal("Please select at least one data to approve.");
            }
        };


        $scope.isDepartmentSelected = false;

        $scope.reportBudgetlist = function () {
            return $filter("filter")($scope.reportBudgetlist, $scope.search);
        };

        $scope.selectedDepartments = [];

        // Function to toggle "Select All"
        $scope.toggleAllDepartments = function (selectAll) {
            $scope.selectedDepartments = [];
            angular.forEach($scope.deptList, function (dept) {
                dept.selected = selectAll;
                if (selectAll) {
                    $scope.selectedDepartments.push(dept.hrmD_DepartmentID);
                }
            });
            $scope.updateReport(); // Update report based on selection
        };

        $scope.updateSelectedDepartments = function () {
            $scope.selectedDepartments = [];

            angular.forEach($scope.deptList, function (dept) {
                if (dept.selected) {
                    $scope.selectedDepartments.push(dept.hrmD_DepartmentID);
                }
            });


            $scope.allSelectedDept = $scope.deptList.every(dept => dept.selected);

            $scope.updateReport();
        };

        //upadte report 
        $scope.updateReport = function () {
            if ($scope.selectedDepartments.length === 0) {
                $scope.isDepartmentSelected = false;
            } else {
                $scope.isDepartmentSelected = true;
            }


            console.log("Selected Departments for Report:", $scope.selectedDepartments);
        };


        $scope.oncmpchangeDept2 = function () {
            $scope.deptList = [];
            // $scope.allBudgetHead = []; 
            var data = {
                "MI_Id": $scope.obj.MI_Id,
                "Flag": $scope.obj.rdopunch

            };
            //call dept based on campany select
            apiService.create("Vms_Budget/GetdeptClient", data).then(function (response) {
                if (response.alldata && response.alldata.length > 0) {
                    console.log('Dept List:', response.deptList);
                    $scope.deptList = response.alldata;
                } else {
                    console.warn('No departments found for the selected company.');
                }
            }).catch(function (error) {
                console.error('Error fetching departments:', error);
            });
        };



        $scope.submitted = false;
        $scope.getApprovalReportdata = function () {
            if ($scope.myForm.$valid) {
                $scope.DepartmentSelected = []; $scope.reportBudgetlist = []; $scope.Budgettotal = [];
                angular.forEach($scope.deptList, function (itm) {
                    if (itm.selected == true) {
                        $scope.DepartmentSelected.push({
                            HRMD_Id: itm.hrmD_Id,
                        });
                    }
                });
                if ($scope.obj.ivrM_Month_Id == undefined || $scope.obj.ivrM_Month_Id == "") {
                    $scope.obj.ivrM_Month_Id = 0;
                }
                var data = {
                    "MI_Id": $scope.obj.MI_Id,
                    "IMFY_ID": $scope.obj.IMFY_Id,
                    "DepartmentSelected": $scope.DepartmentSelected,
                    "MonthId": $scope.obj.ivrM_Month_Id,
                    "Flag": $scope.obj.rdopunch

                };
                apiService.create("Vms_Budget/GetApprovalReport", data).then(function (promise) {
                    if (promise.alldata != null && promise.alldata.length > 0) {
                        //$scope.reportBudgetlist = promise.alldata;
                        // $scope.reportBudgetlist = [];
                        angular.forEach(promise.alldata, function (dev) {
                            if ($scope.reportBudgetlist.length === 0) {
                                $scope.reportBudgetlist.push(dev);
                            }
                            else if ($scope.reportBudgetlist.length > 0) {
                                var intcount = 0;
                                angular.forEach($scope.reportBudgetlist, function (emp) {
                                    if (emp.HRMD_Id === dev.HRMD_Id) {
                                        intcount += 1;
                                    }
                                });
                                if (intcount === 0) {
                                    $scope.reportBudgetlist.push(dev);
                                }
                            }
                        })

                        //$scope.reportBudgetlist

                        angular.forEach($scope.reportBudgetlist, function (bud) {
                            if (bud.HRMD_DepartmentName) {
                                // Clean up the department name
                                var cleanedName = bud.HRMD_DepartmentName.trim().replace(/\s+/g, ' ').replace(/[^\w\s]/gi, '');  // Remove non-alphanumeric characters

                                // Split the department name into individual characters with spaces
                                var transformedName = cleanedName.split('').join(' ');
                                bud.transformedHRMD_DepartmentName = transformedName;  // Store the transformed name
                            }
                        });





                        //working
                        $scope.templist5 = [];

                        var monthNames = [
                            'January', 'February', 'March', 'April', 'May', 'June',
                            'July', 'August', 'September', 'October', 'November', 'December'
                        ];

                        if ($scope.Month_List && $scope.Month_List.length > 0 && $scope.reportBudgetlist) {
                            for (var i = 0; i < $scope.reportBudgetlist.length; i++) {
                                var monthData = $scope.reportBudgetlist[i];
                                var budgetName = monthData.VMBDGHDAMTN_Name;

                                for (var j = 0; j < $scope.Month_List.length; j++) {
                                    var monthName = $scope.Month_List[j].ivrM_Month_Name;

                                    var yValue = monthData[monthName] !== undefined ? monthData[monthName] : 0;

                                    $scope.templist5.push({
                                        label: monthName + ' - ' + budgetName, // Label is month + budgetName for clarity
                                        y: yValue
                                    });
                                }
                            }
                        }



                        //if ($scope.deptList && $scope.deptList.length > 0) {

                        //    $scope.amountMonth = []; 

                        //    for (var i = 0; i < $scope.deptList.length; i++) {
                        //        var dept = $scope.deptList[i];
                        //        for (var j = 0; j < $scope.Month_List.length; j++) {
                        //            var monthName = $scope.Month_List[j].ivrM_Month_Name; 
                        //            var all = promise.overallArray[0].January;
                        //            var all = promise.overallArray[0].February;

                        //            var amount = dept[monthName]; 

                        //            if (amount !== undefined) {
                        //                $scope.amountMonth.push({ label: monthName, amount: yValue });
                        //            }
                        //        }
                        //    }
                        //}


                        //console.log($scope.amountMonth);


                        console.log($scope.reportBudgetlist);

                        angular.forEach($scope.reportBudgetlist, function (ddd) {
                            $scope.templist = [];
                            angular.forEach(promise.alldata, function (dd) {
                                if (dd.HRMD_Id === ddd.HRMD_Id) {
                                    $scope.templist.push(dd);
                                }
                            });
                            ddd.templist = $scope.templist;

                        });


                        //DepartmentArray
                        if (promise.departmentArray != null && promise.departmentArray.length > 0) {
                            angular.forEach($scope.reportBudgetlist, function (ddd) {
                                $scope.deptotal = [];
                                angular.forEach(promise.departmentArray, function (dd) {
                                    if (dd.HRMD_Id === ddd.HRMD_Id) {
                                        $scope.deptotal.push(dd);
                                    }
                                });
                                ddd.deptotal = $scope.deptotal;

                            });
                        }
                        if (promise.overallArray != null && promise.overallArray.length > 0) {
                            $scope.Budgettotal = promise.overallArray;
                        }

                    }


                    if ($scope.templist5.length > 0) {
                        var chart = new CanvasJS.Chart("columnchart", {
                            //height: window.innerHeight * 0.7, 
                            //width: window.innerWidth * 0.7, 
                            axisX: {
                                //labelFontSize: 10,
                                //interval: 2,
                                //labelAngle: -20,
                                //labelFontColor: "black",
                                //labelFontWeight: "bold",
                                labelFontSize: 10,
                                interval: $scope.templist5.length > 40 ? 7 : 4,
                                labelAngle: -40,
                                labelFontColor: "black",
                                labelFontWeight: "bold",
                            },
                            axisY: {
                                labelFontSize: 12,
                            },
                            data: [
                                {
                                    type: "column",
                                    showInLegend: true,
                                    dataPoints: $scope.templist5
                                }
                            ]
                        });

                        chart.render();
                        window.onresize = function () {
                            chart.render();
                        };

                    }

                    else {
                        swal("Record Not Found !");
                    }
                });
            }
            else {
                $scope.submitted = true;
            }

        }


        $scope.exportToExcel = function (export_id) {

            var exportHref = Excel.tableToExcel(export_id, 'sheet name');
            $timeout(function () {
                location.href = exportHref;
            }, 100);

        };
        $scope.otherdaydatechange = function () {
            $scope.deptList = [];
            $scope.obj.MI_Id = "";
            $scope.obj.IMFY_Id = "";
            $scope.reportBudgetlist = [];
            $scope.allSelectedDept = false;
        }


        $scope.printData = function () {

            var innerContents = document.getElementById("printpi").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/baldwin/CumulativeReportPdf.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
                '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
            popupWinindow.document.close();
        };


        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        $scope.Clearid = function () {
            $state.reload();
        };

        $scope.reportBudgetlist;
        //$scope.datagraph = [];
        //if ($scope.Month_List && $scope.Month_List.length > 0) {
        //    for (var i = 0; i < $scope.Month_List.length; i++) {
        //        $scope.datagraph.push({ label: $scope.Month_List[i].IVRM_Month_Name})
        //    }
        //}
        //console.log($scope.datagraph);



        //$scope.datagraph2 = [
        //    { label: "January", y: 100 },
        //    { label: "February", y: 120 },
        //    { label: "March", y: 130 },

        //    { label: "April", y: 100 },
        //    { label: "May", y: 120 },
        //    { label: "June", y: 130 },
        //    // Add more data as necessary
        //];
        //6 = Object { label: "January" }





        // Initialize an array for data points


        //var budgetData = $scope.reportBudgetlist[0];  

        // Loop over the months and prepare data points
        //var months = ["January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December"];
        //months.forEach(function (month) {
        //    if (budgetData[month] !== undefined) {
        //        $scope.datagraph.push({
        //            label: month, 
        //            y: budgetData[month] 
        //        });

        //    }
        //});

        // Now $scope.datagraph is populated with all the months and their respective values

        //columnchart
        //var chart = new CanvasJS.Chart("columnchart", {

        //    axisX: {
        //        labelFontSize: 10,
        //        interval: 1,
        //        labelAngle: -20,
        //        labelFontColor: "black",
        //        labelFontWeight: "bold",
        //        // title:"Class",
        //    },
        //    axisY: {
        //        labelFontSize: 12,
        //        //  title: "Students",
        //    },

        //    data: [
        //        {
        //            type: "column",
        //            showInLegend: true,
        //            dataPoints: $scope.datagraph2,
        //            dataPoints2: $scope.templist5



        //        }
        //    ]


        //});

        //chart.render();



        // Controller code
        $scope.customSearch = function (item) {

            if (!$scope.search1235) return true;


            var searchTerm = $scope.search1235.trim().toLowerCase();
            var transformedName = item.transformedHRMD_DepartmentName ? item.transformedHRMD_DepartmentName.toLowerCase() : '';

            return transformedName.indexOf(searchTerm) !== -1;
        };




    }

})();





