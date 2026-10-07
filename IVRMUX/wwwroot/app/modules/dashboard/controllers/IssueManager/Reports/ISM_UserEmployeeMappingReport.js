(function () {
    'use strict';
    angular
        .module('app')
        .controller('ISM_UserEmployeeMappingController', ISM_UserEmployeeMappingController);

    //ISM_UserEmployeeMappingController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache', 'Excel'];
    //function ISM_UserEmployeeMappingController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache, Excel) {
    ISM_UserEmployeeMappingController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$q', '$stateParams', '$filter', '$sce', '$timeout', 'Excel'];
    function ISM_UserEmployeeMappingController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $q, $stateParams, $filter, $sce, $timeout, Excel) {

        $scope.currentPage = 1;
        $scope.itemsPerPage = 10;
        $scope.currentPage1 = 1;
        $scope.itemsPerPage1 = 10;
        $scope.currentPage2 = 1;
        $scope.itemsPerPage2 = 10;
        $scope.self = true;
        $scope.page1 = "page1";
        $scope.page2 = "page2";
        $scope.search = "";
        $scope.obj = {};
        $scope.employeeList = [];
        $scope.userflg = false;
        
        //=========================================Load data
        $scope.loaddata = function () {
            var pageid = 2;
           
            var data = {
                "get_OnLoadEmpDetailsFlag": 'Dept'
            }
            apiService.create("ISM_UserEmployeeMapping/getdetailsNew", data).then(function (promise) {
                $scope.get_employeelist = promise.get_OnLoadEmpDetails;

            });
        };
        $scope.ids = [];
        $scope.get_employeelist = [];
       // $state.reload();
        $scope.ondatachange = function (user) {
            $scope.get_employeelist = [];
            $scope.ids = [];
            var pageid = 2;
            var data = {
                "get_OnLoadEmpDetailsFlag": $scope.selfassign
                }
            apiService.create("ISM_UserEmployeeMapping/getdetailsNew", data).then(function (promise) {
                $scope.get_employeelist = promise.get_OnLoadEmpDetails;
               // $state.reload();
            });
        };
        $scope.ids = [];
      
        $scope.submitted = false;
        $scope.Report = function (user) {
            $scope.submitted = true;

            if ($scope.myForm.$valid) {
                $scope.temparray = [];
                angular.forEach($scope.get_employeelist, function (itm) {
                    if (itm.selected == true) {
                        $scope.temparray.push({ Hrme_id: itm.Hrme_id })
                    }
                });

                var data = {
                    //"HRME_Id": $scope.Hrme_id
                    "HRMEIDS": $scope.temparray
                };

                apiService.create("ISM_UserEmployeeMapping/getUserEmpMappedReport", data)
                    .then(function (promise) {
                        if (promise.getUserEmpMappedReport.length > 0) {
                           $scope.report = promise.getUserEmpMappedReport;

                            //for displaying grid
                            $scope.ids = [];

                            angular.forEach($scope.report, function (dev) {
                                if ($scope.ids.length === 0) {
                                    $scope.ids.push(dev);
                                } else if ($scope.ids.length > 0) {
                                    var intcount = 0;
                                    angular.forEach($scope.ids, function (emp) {
                                        if (emp.Hrme_id === dev.Hrme_id) {
                                            intcount += 1;
                                        }
                                    });
                                    if (intcount === 0) {
                                        $scope.ids.push(dev);
                                    }
                                }
                            });
                           // console.log($scope.ids);

                            angular.forEach($scope.ids, function (ddd) {
                                $scope.templist = [];

                                angular.forEach($scope.report, function (dd) {
                                    if (dd.Hrme_id === ddd.Hrme_id) {
                                        $scope.templist.push(dd);
                                    }
                                });
                                ddd.datadetails = $scope.templist;

                            });
                            //console.log($scope.ids);
                        } else {
                            swal("No Records Found.");
                        }
                      
                    })
                    .catch(function (error) {
                        console.error("Error fetching report: ", error);
                    });
            } else {
                $scope.submitted = true;
            }
        };


        //$scope.all_checkE = function () {
        //    var checkStatus = $scope.usercheckE;
        //    angular.forEach($scope.get_employeelist, function (itm) {
        //        itm.selected = checkStatus;
        //    });
        //    var count = 0;
        //    var selectedEmployee = null; 
        //    angular.forEach($scope.get_employeelist, function (itm) {
        //        if (itm.selected === true) {
        //            count = count + 1;
        //            selectedEmployee = itm; 
        //        }
        //    });
        //    if (count > 1) {
        //        $scope.self = false;
        //    }
        //    else {
        //        $scope.self = true;
        //    }

        //    if (selectedEmployee) {
        //        $scope.Hrme_id = selectedEmployee.Hrme_id;
        //    } else {
        //        $scope.Hrme_id = null;
        //    }
        //};


        //$scope.togchkbxE = function () {
        //    $scope.usercheckE = $scope.get_employeelist.every(function (options) {
        //        return options.selected;
        //    });

        //    var count = 0;
        //    var selectedEmployee = null; 
        //    angular.forEach($scope.get_employeelist, function (itm) {
        //        if (itm.selected === true) {
        //            count = count + 1;
        //            selectedEmployee = itm; 
        //        }
        //    });

        //    if (count > 1) {
        //        $scope.self = false;
        //    } else {
        //        $scope.self = true;
        //    }

        //    if (selectedEmployee) {
        //        $scope.Hrme_id = selectedEmployee.Hrme_id; 
        //    } else {
        //        $scope.Hrme_id = null; 
        //    }
            
        //};

        //Print
        $scope.printData = function () {
            var innerContents = document.getElementById("employee").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print.css" />' +
                '<link type="text/css" rel="stylesheet" href="css/style.css" />' +
                '<link type="text/css" href="plugins/bootstrap/css/bootstrap.min.css" rel="stylesheet" />' +
                '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
            popupWinindow.document.close();
        }


        //Export To Excel
        $scope.exportToExcel = function (tableemployee) {
            var exportHref = Excel.tableToExcel(tableemployee, 'sheet name');
            $timeout(function () { location.href = exportHref; }, 100);
        }

        $scope.selectedModules = [];
        // selection
        $scope.all_checkE = function (usercheckE) {
            var toggleStatus = usercheckE;
            angular.forEach($scope.get_employeelist, function (itm) {
                itm.selected = toggleStatus;
            });
            $scope.searchValueDesg = '';

        };


        $scope.togchkbxE = function () {

            $scope.usercheckE = $scope.get_employeelist.every(function (itm) {
                return itm.selected;
            });

            $scope.searchValueDesg == '';

        };


        $scope.isOptionsRequired = function () {
            return !$scope.get_employeelist.some(function (options) {
                return options.selected;
            });
        };


        $scope.interacted = function (field) {
            return $scope.submitted;
        };
        $scope.sort = function (keyname) {
            $scope.sortKey = keyname;
            $scope.reverse = !$scope.reverse;
        };

      
        //=========================== cancel
        $scope.cancel = function () {
            $state.reload();
        };
        $scope.cancel = function () {
            angular.forEach($scope.get_employeelist, function (itm) {
                itm.selected = false;  
            });

            $scope.Hrme_id = null;
            $state.reload();
           // $scope.selfassign = false;
            //$state.reload();
        }

       

    }
})();

