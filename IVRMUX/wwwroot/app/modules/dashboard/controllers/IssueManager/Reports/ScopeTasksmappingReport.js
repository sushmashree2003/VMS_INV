(function () {
    'use strict';
    angular
        .module('app')
        .controller('ViewOfDeploymentController', ViewOfDeploymentController);
    ViewOfDeploymentController.$inject = ['$rootScope', '$scope', '$state', '$location', '$q', 'Flash', 'appSettings', 'apiService', '$http', 'superCache', '$filter'];
    function ViewOfDeploymentController($rootScope, $scope, $state, $location, $q, Flash, appSettings, apiService, $http, superCache, $filter) {

        $scope.showview = true;
        $scope.disablesave = false;
        $scope.transnumbconfigurationsettings = [];
        $scope.transnumbconfigurationsettingsassign = {};
        var transnumconfigsettings = JSON.parse(localStorage.getItem("transnumconfigsettings"));
        $scope.obj = {};
        $scope.taskeffort = [];
        $scope.submitted = false;
        $scope.multi_mi = false;
        $scope.ismtcrastO_AssignedDate = new Date();
        $scope.ismtcrtrtO_TransferredDate = new Date();
        $scope.currentdate = new Date();
        $scope.get_plannerlist = [];
        $scope.showapproval = false;

        $scope.get_env = [];
        $scope.arrayText = [];
        $scope.env_id = [];
        //added by new
        $scope.fetchCheckedIDOne = function () {
            if ($scope.liveivrm == true) {
                $scope.env_id.push({
                    VPSD_Environment: "Live IVRM"

                })
            }

        }
        $scope.fetchCheckedIDsTWO = function () {
            if ($scope.livevms == true) {
                $scope.env_id.push({
                    VPSD_Environment: "Live VMS"

                })
            }

        }
        $scope.fetchCheckedIDsThree = function () {
            if ($scope.testivrm == true) {
                $scope.env_id.push({
                    VPSD_Environment: "Test IVRM"

                })
            }

        }
        $scope.fetchCheckedIDsFour = function () {
            if ($scope.testvms == true) {
                $scope.env_id.push({
                    VPSD_Environment: "Test VMS"

                })
            }

        }
        //adda
        $scope.loaddata = function () {
            $scope.env_id = [];
            //$scope.arrayText = [ Name:'hello', 'world'];
            //$scope.addText = function (myText) {
            //    $scope.arrayText.push(myText);
            //};
        };


        $scope.togglecheckboxAll = function (checkalltask) {
            var toggleStatus = checkalltask;
            angular.forEach($scope.get_report, function (slc) {
                slc.checked = toggleStatus;
            });
        };
        $scope.optionToggled = function () {
            $scope.checkalltask = $scope.get_plannerlist.every(function (options) {
                return options.checked;

            });

            $scope.tasklistarray = [];
            angular.forEach($scope.get_tasks, function (qq) {
                if (qq.checked == true) {
                    $scope.tasklistarray.push({ ISMTCR_Id: qq.ismtcR_Id })
                }
            });
        };

        $scope.editplannerdetails = function (objedit) {
            $scope.editflg = 1;
            objedit.editflg = $scope.editflg;
            $scope.getplannerdetails(objedit);
        };


        $scope.all_task = function (atk) {
            var totcheck = 0;
            var toteffort = 0;
            $scope.alltask = atk;
            var toggleStatus = $scope.alltask;
            angular.forEach($scope.get_plannerdetailsapp, function (atck) {
                //if (atck.ISMTPLTA_Status === 'Open') {
                atck.checkedPlan = toggleStatus;
                //}
                if (atck.checkedPlan === true && (atck.plannerStatus === "1" || atck.plannerStatus === 1)) {
                    totcheck = totcheck + 1;
                    toteffort += parseFloat(parseInt(atck.effhrs, 10) + parseInt(atck.effmin, 10) / 60);
                }
                //totcheck = totcheck + 1;
                //toteffort += parseFloat(atck.ISMTPLTA_EffortInHrs);
            });
            $scope.totalcheck = totcheck;
            $scope.totaleffort = toteffort;

            var decimaltimetot = $scope.totaleffort;
            var hrstot = parseInt(Number(decimaltimetot));
            var mintot = Math.round((Number(decimaltimetot) - hrstot) * 60);
            if (hrstot.toString().length === 1) {
                hrstot = '0' + hrstot;
            }
            if (mintot.toString().length === 1) {
                mintot = '0' + mintot;
            }

            if (mintot === 60) {
                hrstot = hrstot + 1;
                mintot = 0;
            }

            $scope.effhrstot = hrstot;
            $scope.minmintot = mintot;
        };

        //========================================= View Mode
      

        $scope.fetchCheckedIDs = function () {
            $scope.checkedIDs = [];
            $scope.checkboxesDataList.forEach((value, index) => {
                if (value.isChecked) {
                    $scope.checkedIDs.push(value.id);
                }
            });
        }
        //=========================
        $scope.allClickenvirment = function () {
            if ($scope.envselectedAll == true) {
                $scope.liveivrm = true;
                $scope.livevms = true;
                $scope.testivrm = true;
                $scope.testvms = true;
                $scope.fetchCheckedIDOne();
                $scope.fetchCheckedIDsTWO();
                $scope.fetchCheckedIDsThree();
                $scope.fetchCheckedIDsFour();
            }
            else {
                $scope.checkedIDs = [];
                $scope.liveivrm = false;
                $scope.livevms = false;
                $scope.testivrm = false;
                $scope.testvms = false;
            }
        }
        $scope.tasksgrid = function (objp) {
            $scope.get_emp_dailyreport = [];
            $scope.arrayuserEmp = [];
            $scope.arrayuserEmp.push({ HRME_Id: $scope.HRME_Id1 });

            var data = {
                // arrayuserEmp: $scope.arrayuserEmp,
                "Fromdate": new Date($scope.start_Date).toDateString(),
                "Todate": new Date($scope.end_Date).toDateString(),
                "envlistarray": $scope.env_id

            };
            apiService.create("ViewOfDeployment/getReport", data).then(function (promise) {
                $scope.plannerid = [];
                $scope.hide_n = false;
                $scope.feed = false;
                if (promise.get_report.length > 0) {
                    $scope.get_report = promise.get_report;

                }
                else {
                    
                        swal("No Record Found..... !!");
                    
                    
                }

            });


        };

        $scope.showflag = false;
        $scope.approvalReport = function (objp) {

            $scope.showflag = true;
            
            $scope.get_plannerlist = [];
            angular.forEach($scope.get_report, function (qq) {
                if (qq.checked == true) {
                    $scope.get_plannerlist.push({ ISMTCR_Title: qq.ISMTCR_Title, ISMTCR_Desc: qq.ISMTCR_Desc, ISMTCR_TaskNo: qq.ISMTCR_TaskNo, CreatedDate: qq.CreatedDate})
                }
            });
            if ($scope.get_plannerlist.length > 0) {
                //   $scope.get_report = promise.get_report;

                //$scope.header_list = [];
                $('#grid123').empty();
        //       $scope.get_plannerlist = true;


                $scope.insarray = [{ name: "ISMTCR_Title" }, { name: " ISMTCR_Desc" }, { name: "ISMTCR_TaskNo" }, { name: "CreatedDate" }];
                $scope.columns1 = [{ field: "ISMTCR_Title", name: "ISMTCR_Title", title: "ISMTCR_Title" }, { field: "ISMTCR_Desc", name: "ISMTCR_Desc", title: "ISMTCR_Desc" }, { field: "ISMTCR_TaskNo", name: "ISMTCR_TaskNo", title: "ISMTCR_TaskNo" }, { field: "CreatedDate", name: "CreatedDate", title: "CreatedDate" }

                ];
                $scope.newarray = [];
                $scope.newarray1 = $scope.newarray;

              $scope.data = $scope.get_plannerlist;

               // if ($scope.get_plannerlist.length > 0) {
                    //var totalrate = 0.00;
                    //var totaltax = 0.00;
                    //var totalamt = 0.00;
                    //$scope.get_Inwardreport = promise.get_Inwardreport;
                    //$scope.presentCountgrid = $scope.get_Inwardreport.length;

                    //angular.forEach($scope.get_report, function (po) {
                    //});

                    //angular.forEach($scope.get_report, function (op) {
                    //    op.columns = [];
                    //    angular.forEach($scope.columns1, function (op123) {

                    //        op.columns.push({ field: "hema" + op.ftI_Id + op123.field, name: "hema" + op.ftI_Id + op123.name, title: op123.title, width: 100 });

                    //    })
                    //    op.title = op.ftI_Name;
                    //})


                    $scope.tempaggary = [];
                    $scope.colarrayall = [{

                        title: "SLNO",
                        template: "<span class='row-number'></span>", width: 75

                    },
                    {
                        name: 'ISMTCR_TaskNo', field: 'ISMTCR_TaskNo', title: 'ISMTCR_TaskNo', width: 150
                    },
                    {
                        name: 'ISMTCR_Title', field: 'ISMTCR_Title', title: 'ISMTCR_Title', width: 150
                    },
                    {
                        name: 'ISMTCR_Desc', field: 'ISMTCR_Desc', title: 'ISMTCR_Desc', width: 150
                    },
                    {
                        name: 'CreatedDate', field: 'CreatedDate', title: 'CreatedDate', width: 250
                    }


                    ];


                    //   $scope.tempaggary.push({ field: 'INVTGRN_Amount', name: 'INVTGRN_Amount', format: "{0:n2}", aggregate: "sum" });

                    ///function (e) { return e.userDetails.UserTypeId == 1 ? false : true; }
                    angular.forEach($scope.get_plannerlist, function (obj) {
                        $scope.colarrayall.push(obj);
                    })



                    console.log($scope.get_plannerlist);

                    console.log($scope.colarrayall);

                    //$scope.txtdata = "TIME :" + " " + $filter('date')(new Date(), 'HH:mm:ss') + "  " + "," + " " + "USERNAME :" + " " + $scope.usrname;

               //     console.log($scope.txtdata);
                    $scope.aaaa = [{
                        title: $scope.txtdata,
                        columns: $scope.colarrayall
                    }]
                    console.log($scope.data);
                    var gridall;


                    $(document).ready(function () {
                        initGridall();

                    });

                    function initGridall() {
                        gridall = $("#grid123").kendoGrid({
                            toolbar: ["excel", "pdf"],
                            excel: {
                                fileName: "ScopeMappingReport.xlsx",
                                proxyURL: "",
                                filterable: true,


                            },


                            pdf: {
                                fileName: "ScopeMappingReport.pdf"
                            },
                            dataSource: {

                                //type: "odata",
                                //transport: {
                                //    read: "https://demos.telerik.com/kendo-ui/service/Northwind.svc/Products"
                                //},
                                data: $scope.data,
                                aggregate: $scope.tempaggary

                            },
                            sortable: true,
                            pageable: false,
                            groupable: false,
                            filterable: true,
                            columnMenu: true,
                            reorderable: true,
                            resizable: true,

                            columns: $scope.aaaa,
                            dataBound: function () {
                                var rows = this.items();
                                $(rows).each(function () {
                                    var index = $(this).index() + 1;
                                    var rowLabel = $(this).find(".row-number");
                                    $(rowLabel).html(index);
                                });
                            }
                        }).data("kendoGrid");
                        gridall.setOptions({
                            sortable: true
                        });
                   // }

                }

            }
        };

        $scope.cancel = function () {
            $state.reload();
        };
    }

})();

    

    