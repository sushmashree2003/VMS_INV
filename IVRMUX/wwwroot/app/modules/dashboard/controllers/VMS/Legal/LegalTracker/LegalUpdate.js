(function () {
    'use strict';
    angular.module('app').controller('LegalUpdateController', LegalUpdateController)

    LegalUpdateController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'Excel', '$timeout', '$filter', 'superCache', 'myFactoryvisitor', '$q', '$sce']
    function LegalUpdateController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, Excel, $timeout, $filter, superCache, myFactoryvisitor, $q, $sce) {

        $scope.showgrd = false;
        $scope.hstep = 1;
        $scope.mstep = 1;
        $scope.search = "";
        $scope.search1 = "";
        $scope.currentPage = 1;
        $scope.currentPage1 = 1;
        $scope.itemsPerPage = 15;
        $scope.itemsPerPage1 = 10;
        $scope.itemsPerPage11 = 5;
        $scope.currentPage11 = 1;
        //previous date block
        $scope.entry_date = new Date();
        $scope.entry_date.setDate($scope.entry_date.getDate());
        $scope.obj = {};

        var vm = this;
        vm.gridOptions = {};





        $scope.removeNewDetail = function (index, data) {
            var newItemNo = $scope.employeenameschange.length - 1;
            $scope.employeenameschange.splice(index, 1);
            if (data.hrmE_Id > 0) {
                $scope.DeleteDocumentDataOthers(data);
            }
        };

        $scope.exptoex = function () {

            window.open("https://dcampusstrg.blob.core.windows.net/files/17/ISM_Attachments/e7ec5bd7-eac2-494b-92ff-23a98194b12b.xlsx");

        };


        $scope.loaddata = function () {
            var pageid = 2;
            apiService.getURI("LegalUpdate/loaddata", pageid).then(function (promise) {
                $scope.countdetails = promise.countdetails;
            });
        };


        //////////////////////\\\\\\\\\\\\\\\\\\\\\\\\\\



        //$scope.saveData1 = function (gridOptions) {
        //    $scope.submitted = true;            

        //    if (Array.isArray(gridOptions) && gridOptions.length > 0) {             
        //        $scope.LegalUpdate = gridOptions.slice();
        //        $scope.LegalUpdat = [];
        //        if ($scope.LegalUpdate.length > 0) {
        //            angular.forEach($scope.LegalUpdate, function (itm) {
        //                    $scope.LegalUpdat.push({
        //                        LU_CaseType: itm.CaseType,
        //                        LU_FillingNumber: itm.FillingNumber,
        //                        LU_FillingDate: itm.FillingDate,
        //                        LU_RegistrationNumber: itm.RegistrationNumber,
        //                        LU_RegistrationDate: itm.RegistrationDate,
        //                        LU_CNRNumber: itm.CNRNumber,
        //                        LU_FirstHearingDate: itm.FirstHearingDate,
        //                        LU_NextHearingDate: itm.NextHearingDate,
        //                        LU_CaseStatus: itm.CaseStatus,
        //                        LU_StageofCase: itm.Stageofcase,
        //                        LU_CourtNumber: itm.CourtNumberandJudge,
        //                        LU_UnderActs: itm.UnderActs,
        //                        LU_UnderSection: itm.UnderSection,
        //                        LU_Judge: itm.Judge,
        //                        LU_BusinessOnDate: itm.BusinessOnDate,
        //                        LU_HearingDate: itm.HearingDate,
        //                        LU_PurposeofHearing: itm.PurposeofHearing,
        //                        LU_ProcessId: itm.ProcessId,
        //                        LU_ProcessDate: itm.ProcessDate,
        //                        LU_ProcessTittle: itm.ProcessTittle,
        //                        LU_PartyName: itm.PartyName,
        //                        LU_IssuedProcess: itm.IssuedProcess,
        //                        LU_PetitionerandAdvocate: itm.PetitionerAndAdvocate,
        //                        LU_RespondentandAdvocate: itm.RespondentAndAdvocate
        //                    });
        //            });
        //        }

        //        var data = {
        //            "LegalUpdate": $scope.LegalUpdat
        //        };


        //        apiService.create("LegalUpdate/saveData1", data)
        //            .then(function (promise) {
        //                if (promise.returnvalues === "Add") {
        //                    swal("Record Saved Successfully");
        //                    $state.reload();
        //                } else if (promise.returnvalues === "duplicate") {
        //                    swal("Record already exists");
        //                    $state.reload();
        //                } else 
        //                {
        //                    swal("Failed to save record");
        //                }                       
        //            })
        //            .catch(function (error) {

        //                console.error("Error:", error);
        //                swal("Error occurred while saving record. Please try again later.");
        //            });
        //    } else {
        //        swal("Kindly fill mandatory fields");
        //    }
        //};

        $scope.saveData1 = function (gridOptions) {
            $scope.submitted = true;

            if (Array.isArray(gridOptions) && gridOptions.length > 0) {
                $scope.LegalUpdate = [];
                angular.forEach(gridOptions, function (itm) {
                    $scope.LegalUpdate.push({
                        LU_CaseType: itm.CaseType,
                        LU_FillingNumber: itm.FillingNumber,
                        LU_FillingDate: itm.FillingDate,
                        LU_RegistrationNumber: itm.RegistrationNumber,
                        LU_RegistrationDate: itm.RegistrationDate,
                        LU_CNRNumber: itm.CNRNumber,
                        LU_FirstHearingDate: itm.FirstHearingDate,
                        LU_NextHearingDate: itm.NextHearingDate,
                        LU_CaseStatus: itm.CaseStatus,
                        LU_StageofCase: itm.Stageofcase,
                        LU_CourtNumber: itm.CourtNumberandJudge,
                        LU_UnderActs: itm.UnderActs,
                        LU_UnderSection: itm.UnderSection,
                        LU_Judge: itm.Judge,
                        LU_BusinessOnDate: itm.BusinessOnDate,
                        LU_HearingDate: itm.HearingDate,
                        LU_PurposeofHearing: itm.PurposeofHearing,
                        LU_ProcessId: itm.ProcessId,
                        LU_ProcessDate: itm.ProcessDate,
                        LU_ProcessTittle: itm.ProcessTittle,
                        LU_PartyName: itm.PartyName,
                        LU_IssuedProcess: itm.IssuedProcess,
                        LU_PetitionerandAdvocate: itm.PetitionerAndAdvocate,
                        LU_RespondentandAdvocate: itm.RespondentAndAdvocate
                    });
                });

                var data = {
                    "LegalUpdate": $scope.LegalUpdate
                };

                apiService.create("LegalUpdate/saveData1/", data)
                    .then(function (promise) {
                        console.log(promise);
                        if (promise.returnvales === "Add") {
                            swal("Record Saved Successfully. Number of Records saved : " + promise.recordsSavedCount);
                            $state.reload();
                        } else if (promise.returnvales === "duplicate") {
                            swal("Record already exists");
                            $state.reload();
                        } else {
                            swal("Record Saved Successfully. Number of Records saved : " + promise.recordsSavedCount);
                        }
                    })
                    .catch(function (error) {
                        console.error("Error:", error);
                        swal("Error occurred while saving record. Please try again later.");
                    });
            } else {
                swal("Kindly fill mandatory fields");
            }
        };



        $scope.cancel = function () {
            $state.reload();
        };

        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        $scope.interacted12 = function (field) {
            return $scope.submitted;
        };
        $scope.interacted21 = function (field) {
            return $scope.submitted;
        };

        // /////////////////////import to excel///////////////////////////

        $scope.excel1 = false;
        $scope.excelhide2 = function () {
            $scope.excel1 = true;
        }
        $scope.excel = false;
        $scope.excelhide = function () {
            $scope.excel = true;
        }
        $scope.SelectedFileForUploadzd = [];
        $scope.selectFileforUploadzd = function (input) {

            $scope.SelectedFileForUploadzd = input.files;

            if (input.files && input.files[0]) {

                if (input.files[0].type == "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet") {

                    //once return success 
                    var reader = new FileReader();
                    reader.readAsDataURL(input.files[0]);

                }
                else {
                    swal("Please Upload the .xlsx file");
                    return;
                }
            }

        }
        $rootScope.validateForm = function () {

            console.log($scope.opts.data);
            var data1 = {
                "newlstget1": $scope.opts.data
            }
            apiService.create("LegalUpdate/validation", data1).then(function (promise) {
                if (promise.stuStatus != "" && promise.stuStatus != null) {
                    swal(promise.stuStatus);
                    $elm.val(null);
                    $scope.opts.data = null;
                } else {
                    $scope.opts.data = $scope.opts.data;
                }
            });
        }


        // /////////////////////import to excel///////end////////////////////





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



angular
    .module('app').filter('keys', function () {

        return function (input) {
            if (!input) {
                return [];
            }
            delete input.$$hashKey;
            return Object.keys(input);
        }

    })

angular
    .module('app').directive("fileread", ['$rootScope', 'apiService', function ($rootScope, apiService) {

        return {
            scope: {
                opts: '='

            },
            link: function ($scope, $elm, $attrs) {

                $elm.on('change', function (changeEvent) {

                    var reader = new FileReader();

                    reader.onload = function (evt) {
                        $scope.$apply(function () {
                            var data = evt.target.result;

                            var workbook = XLSX.read(data, { type: 'binary' });

                            var headerNames = XLSX.utils.sheet_to_json(workbook.Sheets[workbook.SheetNames[0]], { header: 1 })[0];

                            var data = XLSX.utils.sheet_to_json(workbook.Sheets[workbook.SheetNames[0]]);

                            if (data.length == 0) {

                                swal("Excel Sheet is Empty");
                                $elm.val(null);
                                $scope.opts.data = null;
                                return;

                            }
                            else {
                                $scope.opts = {};
                                $scope.opts.columnDefs = [];
                                headerNames.forEach(function (h) {
                                    $scope.opts.columnDefs.push({ field: h });
                                });
                                //dfsd
                                $scope.opts.data = data;
                                // $rootScope.validateForm();

                                console.log($scope.opts.data);
                                var data1 = {
                                    "newlstget1": $scope.opts.data
                                }
                                apiService.create("LegalUpdate/validation", data1).then(function (promise) {
                                    if (promise.stuStatus != "" && promise.stuStatus != null) {
                                        swal(promise.stuStatus);
                                        $elm.val(null);
                                        $scope.opts.data = null;
                                    } else {
                                        $scope.opts.data = $scope.opts.data;
                                    }
                                });


                                //$elm.val(null);
                            }
                        });
                    };
                    reader.readAsBinaryString(changeEvent.target.files[0]);

                });
            }
        }
    }]);