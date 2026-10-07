(function () {
    'use strict';
    angular.module('app').controller('ViewOfDeployment_ApprovalController', ViewOfDeploymentController);
    ViewOfDeploymentController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'Excel', '$timeout', '$filter', 'superCache', 'myFactoryvisitor', '$q', '$sce'];
    function ViewOfDeploymentController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, Excel, $timeout, $filter, superCache, myFactoryvisitor, $q, $sce) {

        var date = new Date();
        $scope.date_select = date;

        var paginationformasters;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
        }
        else {
            paginationformasters = 0;
        }
        $scope.transnumbconfigurationsettings = [];
        $scope.transnumbconfigurationsettingsassign = {};
        var transnumconfigsettings = JSON.parse(localStorage.getItem("transnumconfigsettings"));
        for (var i = 0; i < transnumconfigsettings.length; i++) {
            if (transnumconfigsettings.length > 0) {
                if (transnumconfigsettings[i].imN_Flag === "ISMTASK") {
                    $scope.transnumbconfigurationsettingsassign = transnumconfigsettings[i];
                }
            }
        }

        $scope.loaddata = function () {
            $scope.currentPage = 1;
            $scope.currentPage1 = 1;
            $scope.itemsPerPage = paginationformasters;
            $scope.itemsPerPage1 = 10;
            $scope.search = "";
            var pageid = 2;
            apiService.getURI("ViewOfDeployment/getloaddata", pageid).then(function (promise) {
                if (promise != null && promise.get_client.length > 0) {
                    $scope.get_client = promise.get_client;
                    $scope.version_list = [];
                    // console.log($scope.get_client)
                }
                else {
                    swal("No Hosting List Found", "Kindly Map The Hosting For Client Wise", "warning")
                }

            });

        };
        //Hosting change
        $scope.OnChangehost = function () {
            var data = {
                "IVRMCH_Id": $scope.obj.ivrmcH_Id.ivrmcH_Id
            }
            apiService.create("ViewOfDeployment/GetOnchangeData", data).then(function (promise) {
                if (promise != null && promise.version_list.length > 0) {
                    $scope.version_list = promise.version_list;
                }
                else {
                    swal("No Versions Found For This Hosting", "No Deployments Was Done", "warning");
                    $scope.version_list = '';
                }
            });
        };
        

        $scope.get_task_list = function () {
            $scope.temp_a_count = [];
            $scope.total_p_tasks = [];
            $scope.temp_Approved = 0;
            $scope.scope_pending = 0;
            $scope.get_scope_approval_tasklist = [];
            var data = {
                "IVRMCH_Id": $scope.obj.ivrmcH_Id.ivrmcH_Id,
                "IVRMVT_Id": $scope.obj.ivrmvT_Id.ivrmvT_Id
            };
            apiService.create("ViewOfDeployment/get_scope_tasklist_for_approval", data).then(function (promise) {
                if (promise != null && promise.get_scope_approval_tasklist.length > 0) {
                    $scope.get_scope_approval_tasklist = promise.get_scope_approval_tasklist;

                    $scope.total_tasks = $scope.get_scope_approval_tasklist.length;

                    angular.forEach($scope.get_scope_approval_tasklist, function (task) {
                        if (task.IVRMVFD_Status =='Approved') {
                            $scope.temp_a_count.push(task);
                        }
                      
                    });
                    angular.forEach($scope.get_scope_approval_tasklist, function (task) {
                       
                        if (task.IVRMVFD_Status == 'Pending') {
                            $scope.total_p_tasks.push(task);
                        }
                    });
                    $scope.temp_Approved = $scope.temp_a_count.length;
                    $scope.scope_pending = $scope.total_p_tasks.length;
                   
                }
                else {
                    swal("No Task List Found"," ", "cancel");
                }
            });
        }


        $scope.approve_scope = function (dystring) {
            $scope.tem_scope_list = [];
            angular.forEach($scope.get_scope_approval_tasklist, function (task) {
                if (task.check) {
                    $scope.tem_scope_list.push(task);
                }
            });
            if ($scope.tem_scope_list.length > 0) {

                if (dystring =='Approve') {
                    angular.forEach($scope.tem_scope_list, function (task) {
                        task.check = true;
                    });
                }
                else {
                    angular.forEach($scope.tem_scope_list, function (task) {
                        task.check = false;
                    });
                }
                var data = {
                    Approved_rejected_socpe_tasks: $scope.tem_scope_list
                };
                swal({
                    title: "Are you sure?",
                    text: "Do You Want To " + dystring + " Selected Test Cases  ....?" + "  \n  Total Selected : " + $scope.tem_scope_list.length +"",
                    type: "warning",
                    showCancelButton: true,
                    confirmButtonColor: "#DD6B55",
                    confirmButtonText: "Yes, " + dystring + " ",
                    cancelButtonText: "Cancel",
                    closeOnConfirm: false,
                    closeOnCancel: true
                },
                    function (isConfirm) {
                        if (isConfirm) {
                            apiService.create("ViewOfDeployment/approve_reject_scope", data).then(function (promise) {
                                if (promise != null && promise.saved_count > 0 && promise.returnval) {
                                    swal("Selected Test Cases Was" + dystring +" ", "Total Tasks Approved : " + promise.saved_count + "", "success")
                                    $scope.get_task_list();
                                }
                                else {
                                    swal("Error  Occurred While Saving / Updating Records", "", "warning")
                                    $state.reload();
                                }
                            });
                        }
                        else {
                            swal("Canceled!!!");
                        }
                    });


                var data = {
                    Approved_rejected_socpe_tasks: $scope.tem_scope_list
                };
                //apiService.create("ViewOfDeployment/approve_reject_scope", data).then(function (promise) {

                //});
            }
            else {
                
                swal('Kindly Select Atleast One Task For Approval / Reject', 'Selected Count :' + $scope.tem_scope_list.length + '', 'info');
            }
        }


        $scope.selectall = function (status) {
            let foundClose = false;

            angular.forEach($scope.get_scope_approval_tasklist, function (task) {
                if (task.IVRMVFD_Status === 'Close' && task.IVRMVFD_ApprovedFlag==0) {
                    task.check = status;
                    foundClose = true;
                }
            });

            if (!foundClose) {
                $scope.checkall = 0;
                swal("No tasks with status 'Close'", "Kindly Update The Status In The  Scope Of Deployment", "warning");
            }
        };
        $scope.view_test_case_file = function (filepath, filename) {
            $scope.Documentname = filename;
            const extension = filename.split('.').pop().toLowerCase();
            if (extension == 'pdf') {
                var imagedownload = filepath;
                $scope.content = "";
                var fileURL = "";
                var file = "";
                document.getElementById("pdfviewdd").innerHTML = "";
                $http.get(imagedownload, { responseType: 'arraybuffer' })
                    .success(function (response) {
                        file = new Blob([(response)], { type: 'application/pdf' });
                        fileURL = URL.createObjectURL(file);
                        $scope.content = $sce.trustAsResourceUrl(fileURL);
                        var htmlElements = '<embed id="pdfview" src="' + $scope.content + '"  style="width: 100%;" height="1000" type="application/pdf" />';
                        document.getElementById("pdfviewdd").innerHTML = htmlElements;
                        $('#showpdf').modal('show');
                    });

            }
            else if (extension == 'xlsx' || extension == 'xls') {
                //view excel files                var docpath = '';
                docpath = "https://view.officeapps.live.com/op/view.aspx?src=" + encodeURIComponent(filepath);
                $scope.detailFrame = $sce.trustAsResourceUrl(docpath);
                $('#myModaldocx').modal('show');
            }
            else {
                swal("Unsupported File Format To View", "", "warning");
            }
        };


        $scope.cancel = function () {
            $state.reload();
        }
    }
})();

