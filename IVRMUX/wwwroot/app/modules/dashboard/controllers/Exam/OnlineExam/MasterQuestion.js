

(function () {
    'use strict';
    angular
.module('app')
.controller('MasterQuestionController', MasterQuestionController)

    MasterQuestionController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$stateParams', 'Excel', '$timeout', 'superCache', '$filter']
    function MasterQuestionController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $stateParams, Excel, $timeout, superCache, $filter) {

        $scope.groups = [{ title: 'Dynamic Group Header - 1', content: 'Dynamic Group Body - 1' },
               { title: 'Dynamic Group Header - 2', content: 'Dynamic Group Body - 2' },
               { title: 'Dynamic Group Header - 3', content: 'Dynamic Group Body - 3' },
               { title: 'Dynamic Group Header - 4', content: 'Dynamic Group Body - 4' }];

        $scope.searc_button = true;
        $scope.sortKey = 'pamoeQ_Id';
        $scope.sortReverse = true;
        $scope.searchValue = "";
        $scope.answer = "";
        $scope.show_ansOption = false;
        var paginationformasters;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
        }

        //-------------------Load Data
        $scope.loaddata = function () {
            var pageid = 2;
            apiService.getURI("MasterQuestion/getloaddata", pageid).
            then(function (promise) {
                $scope.currentPage = 1;
                $scope.itemsPerPage = paginationformasters;
                $scope.getQuestiondetails = promise.getQuestiondetails;
                $scope.presentCountgrid = $scope.getQuestiondetails.length;
                $scope.result = promise.result;
                $scope.getclass = promise.getclass;
                $scope.getSubjects = promise.getSubjects;
                $scope.getAnsOptions = promise.getAnsOptions;
                // $scope.getQdetails = promise.getQdetails;
                $scope.getFQuestiondetails = promise.getFQuestiondetails;


                $scope.getFQOptiondetails = promise.getFQOptiondetails;
                $scope.test = promise.getQuestiondetails;
                $scope.Option_Qus = [];
                $scope.presentCountgrid2 = promise.result.length;
                $scope.presentCountgrid3 = promise.getFQOptiondetails.length;


                //angular.forEach($scope.getFQOptiondetails, function (Qus_op) {
                //    angular.forEach($scope.test, function (Qus) {
                //        if (Qus.pamoeQ_Id == Qus_op.pamoeQ_Id) {
                //            var index = $scope.test.indexOf(Qus);
                //            $scope.test.splice(index, 1);
                //        }
                //    })

                //})
            })
        };


        //------------------1st Tab 
        $scope.savedata = function () {
            $scope.submitted1 = true;
            $scope.submitted2 = false;
            $scope.submitted3 = false;
            debugger;
            if ($scope.myForm.$valid) {
                var data = {
                    "PAMOEQ_Question": $scope.pamoeQ_Question,
                    "PAMOEQ_QuestionDesc": $scope.pamoeQ_QuestionDesc,
                    "PAMOEQ_Marks": $scope.pamoeQ_Marks,
                    "PAMOEQ_Id": $scope.pamoeQ_Id,
                    "ISMS_Id": $scope.ismS_Id,
                }
                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                }
                debugger;
                apiService.create("MasterQuestion/savedetails", data).
                             then(function (promise) {

                                 if (promise.returnval == true) {
                                     if (promise.pamoeQ_Id == 0 || promise.pamoeQ_Id < 0) {
                                         swal('Record saved successfully');
                                     }
                                     else if (promise.pamoeQ_Id > 0) {
                                         swal('Record updated successfully');
                                     }
                                 }
                                 else if (promise.returnduplicatestatus == 'Duplicate') {
                                     swal('Record already exist');
                                 }
                                 else {
                                     if (promise.pamoeQ_Id == 0 || promise.pamoeQ_Id < 0) {
                                         swal('Failed to save, please contact administrator');
                                     }
                                     else if (promise.pamoeQ_Id > 0) {
                                         swal('Failed to update, please contact administrator');
                                     }
                                 }
                                 $state.reload();
                             })
            }
            else {
                $scope.submitted1 = true;
            }
        };

        //2nd tab save

        $scope.savedataclass = function () {
            $scope.submitted2 = true;
            $scope.submitted1 = false;
            $scope.submitted3 = false;
            debugger;
            if ($scope.myForm2.$valid) {
                var data = {
                    "PAMOEQ_Question": $scope.pamoeQ_Question,
                    "ASMCL_Id": $scope.asmcL_Id,
                    "PAMOEQ_Id": $scope.PAMOEQ_Id.pamoeQ_Id,
                }
                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                }
                debugger;
                apiService.create("MasterQuestion/savedataclass", data).
                    then(function (promise) {

                        if (promise.returnval == true) {
                            if (promise.pamoeQ_Id >0 ) {
                                swal('Record saved successfully');
                            }
                            else {
                                swal('Record updated successfully');
                            }
                        }
                        else if (promise.returnduplicatestatus == 'Duplicate') {
                            swal('Record already exist');
                        }
                        else {
                            if (promise.pamoeQ_Id == 0 || promise.pamoeQ_Id < 0) {
                                swal('Failed to save, please contact administrator');
                            }
                            else if (promise.pamoeQ_Id > 0) {
                                swal('Failed to update, please contact administrator');
                            }
                        }
                        $state.reload();
                    })
            }
            else {
                $scope.submitted2 = true;
            }
        };
        
        $scope.editQuestion = function (id) {
            debugger;
            var data = {
                "pamoeQ_Id": id
            }

            apiService.create("MasterQuestion/editQuestion", data).then(function (promise) {
                if (promise.editQus.length > 0) {
                    $scope.pamoeQ_Question = promise.editQus[0].pamoeQ_Question;
                    $scope.pamoeQ_QuestionDesc = promise.editQus[0].pamoeQ_QuestionDesc;
                    $scope.pamoeQ_Marks = promise.editQus[0].pamoeQ_Marks;
                    $scope.pamoeQ_Id = promise.editQus[0].pamoeQ_Id;
                    $scope.ismS_Id = promise.editQus[0].ismS_Id;
                }
            })

        }

        $scope.editQuestion1 = function (id, asmcL_Id) {
            debugger;
            
            //apiService.create("MasterQuestion/editQuestion", data).then(function (promise) {
            //    if (promise.editQus.length > 0) {
            //        $scope.pamoeQ_Question = promise.editQus[0].pamoeQ_Question;
            //        $scope.pamoeQ_QuestionDesc = promise.editQus[0].pamoeQ_QuestionDesc;
            //        $scope.pamoeQ_Marks = promise.editQus[0].pamoeQ_Marks;
            //        $scope.pamoeQ_Id = promise.editQus[0].pamoeQ_Id;
            //        $scope.ismS_Id = promise.editQus[0].ismS_Id;
            //    }
            //})

            angular.forEach($scope.result, function (opq) {
                if (id == opq.pamoeQ_Id) {
                    $scope.asmcL_Id = opq.asmcL_Id;
                    $scope.PAMOEQ_Id = opq;
                }
            })
        }




        $scope.cancel = function () {
            $state.reload();
        }
        
        $scope.sort = function (key) {
            $scope.sortKey = key;   //set the sortKey to the param passed
            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
        }



        $scope.interacted = function (field) {
            return $scope.submitted1;
        };

        $scope.interacted2 = function (field) {
            return $scope.submitted2;
        };
        $scope.interacted3 = function (field) {
            return $scope.submitted3;
        };



        $scope.IsHidden = true;
        $scope.ShowHide = function () {
            $scope.IsHidden = $scope.IsHidden ? false : true;
        }

        $scope.search_box = function () {
            if ($scope.searchValue != "" || $scope.searchValue != null) {
                $scope.searc_button = false;
            }
            else {
                $scope.searc_button = true;
            }
        }
        $scope.filterValue2 = function (obj) {
            return (angular.lowercase(obj.asmcL_ClassName)).indexOf(angular.lowercase($scope.searchValue2)) >= 0 ||
                (angular.lowercase(obj.pamoeQ_Marks)).indexOf(angular.lowercase($scope.searchValue2)) >= 0;
        }

      
        $scope.optionToggled = function (user) {
            debugger;
            $scope.ques_id = user.pamoeQ_Id;
            $scope.xyz = $scope.getQuestiondetails.every(function (itm) { return itm.xyz; });
            $scope.show_ansOption = true;
        };
        //---------------------------------------
        $scope.toggleAllC = function () {
            angular.forEach($scope.getAnsOptions, function (subj) {
                subj.abc = $scope.allC;
            })
        };

        $scope.optionToggledC = function () {
            $scope.allC = $scope.getAnsOptions.every(function (itm) { return itm.abc; });
        };
       



        $scope.addNew = function (totalgrid) {
            $scope.totalgrid = [];
            var LMBANO_No = '';
            if ($scope.getAnsOptions != null || $scope.getAnsOptions != '') {
                var a = $scope.getAnsOptions;
                for (var i = 0; i < a; i++) {
                    $scope.totalgrid.push({
                        PAMOEQOA_Id: '',
                        PAMOEQOA_Option: '',
                        PAMOEQOA_OptionCode: '',
                    });
                }
                $scope.show_ansOption = true;
            }
        };
        

        $scope.optionChange = function () {
            debugger;
            var data = {
                "OptionType": $scope.pamoeaO_Id,
            }
            apiService.create("MasterQuestion/optionChange", data).
                   then(function (promise) {
                       debugger;
                       $scope.getAnsOptions = promise.noopt;
                       $scope.addNew();
                   })
        }

        $scope.att = {};

        $scope.savedata1 = function (att) {
            $scope.submitted3 = true;
            $scope.submitted1 = false;
            $scope.submitted2 = false;
            debugger;
            if ($scope.myForm3.$valid) {
                var cnt = 0;
                $scope.test = $scope.totalgrid;
                for (var m = 0; m < $scope.totalgrid.length; m++) {
                    var stu_id1 = $scope.totalgrid[m].PAMOEQOA_Option;
                    var stu_id2 = $scope.totalgrid[m].PAMOEQOA_OptionCode;
                    var already_cnt = 0;
                    angular.forEach($scope.test, function (itm1) {
                        if (itm1.PAMOEQOA_Option == stu_id1 && itm1.PAMOEQOA_OptionCode == stu_id2) {
                            already_cnt += 1;
                        }
                    })
                    if (already_cnt == 1) {

                    }
                    else {
                        cnt += 1;
                    }
                }

                //angular.forEach($scope.getFQuestiondetails, function (itm1) {
                //    if (itm1.PAMOEQOA_Option == stu_id1 && itm1.PAMOEQOA_OptionCode == stu_id2) {
                //        already_cnt += 1;
                //    }
                //})




                var data = {
                    "PAMOEQ_Id": $scope.att.xyz,
                    seleted_Ans: $scope.totalgrid,
                    "Answer": $scope.PAMOEQOA_AnswerFlag
                }

                if (cnt < 1) {
                    apiService.create("MasterQuestion/savedetails1", data).then(function (promise) {
                        if (promise.returnval == true) {
                            if (promise.pamoeqoA_Id == 0 || promise.pamoeqoA_Id < 0) {
                                swal("Record saved Successfully");
                            } else if (promise.pamoeqoA_Id > 0) {
                                swal("Record Upadte Successfully");
                            }
                        }
                        else if (promise.returnduplicatestatus == 'Duplicate') {
                            swal('Record already exist');
                        }
                        else {
                            if (promise.pamoeqoA_Id == 0 || promise.pamoeqoA_Id < 0) {
                                swal('Failed to save, please contact administrator');
                            }
                            else if (promise.pamoeqoA_Id > 0) {
                                swal('Failed to update, please contact administrator');
                            }
                        }
                        $scope.cancel1();
                        $scope.loaddata();
                    })
                }
                else {
                    swal('Duplicate Answer Options Entered');
                }
              
            }
            else {
                $scope.submitted3 = true;
            }
        };


        $scope.addColumn = function (role, indexx, totalgrid) {

            angular.forEach(totalgrid, function (subscription, index) {
                if (indexx != index)
                    subscription.PAMOEQOA_AnswerFlag = false;
            });
        }



        $scope.onformclick = function (pamoeQ_Id) {
            debugger;
            var data = {
                "PAMOEQ_Id": pamoeQ_Id
            }
            apiService.create("MasterQuestion/optiondetails", data).
               then(function (promise) {
                   $scope.getoptiondetails = promise.getoptiondetails;
                   $scope.Question = $scope.getoptiondetails[0].pamoeQ_Question;
               })
        }

        $scope.cancel1 = function (user) {
            $scope.ques_id = "";
           $scope.loaddata();
            $scope.show_ansOption = false;
        }


        $scope.search_box1 = function () {
            if ($scope.searchValue1 != "" || $scope.searchValue1 != null) {
                $scope.searc_button1 = false;
            }
            else {
                $scope.searc_button1 = true;
            }
        }
        $scope.filterValue1 = function (obj) {
            return (angular.lowercase(obj.pamoeQ_Question)).indexOf(angular.lowercase($scope.searchValue)) >= 0 ||
                (angular.lowercase(obj.pamoeQ_QuestionDesc)).indexOf(angular.lowercase($scope.searchValue)) >= 0 ||
                (JSON.stringify(obj.pamoeQ_Marks)).indexOf($scope.searchValue) >= 0;
        }



        $scope.DeletRecord = function (employee, SweetAlert) {
            $scope.editEmployee = employee;
            var data = {
                "PAMOEQ_Id": $scope.editEmployee,
            }

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
                        apiService.create("MasterQuestion/Deletedetails", data).
                            then(function (promise) {

                                if (promise.returnval == true) {
                                    swal('Record Deleted Successfully');
                                }
                                else {
                                    swal('Record Not Deleted Successfully');
                                }
                                $state.reload();
                            })
                    }
                    else {
                        swal("Record Deletion Cancelled");
                    }
                });
        }




    }
})();