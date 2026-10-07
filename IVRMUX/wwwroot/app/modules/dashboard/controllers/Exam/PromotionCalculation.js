
(function () {
    'use strict';
    angular
.module('app')
.controller('PromotionCalculationController', PromotionCalculationController)

    PromotionCalculationController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http']
    function PromotionCalculationController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http) {



        $scope.BindData = function () {
            apiService.getDATA("PromotionCalculation/Getdetails").
       then(function (promise) {

           $scope.yearlt = promise.yearlist;
         //  $scope.clslist = promise.classlist;
         //  $scope.seclist = promise.seclist;
         //  $scope.exsplt = promise.exmstdlist;
       })
        };
        $scope.submitted = false;
        $scope.saveddata = function (asmaY_Id, asmcL_Id, asmS_Id, emE_Id) {
            if ($scope.myForm.$valid) {
                var data = {
                    "ASMAY_Id": asmaY_Id,
                    "ASMCL_Id": asmcL_Id,
                    "ASMS_Id": asmS_Id,
                 //   "EME_Id": emE_Id,
                }
                apiService.create("PromotionCalculation/Calculation", data).
           then(function (promise) {

             //  if (promise.returnval === true) {

                   swal('Promotion Calculated Successfully');
              // }
             //  else {

             //      swal('Promotion Not Calculated !!!');
             //  }
               $scope.clear();
           })

            }
            else {
                $scope.submitted = true;
            }
        };

        $scope.cancel = function () {
            $scope.asmcL_Id = ""
            $scope.emcA_Id = ""
            $scope.asmaY_Id = ""
            $scope.emG_Id = ""
            $scope.asmS_Id = ""
            $scope.subjectlt = ""
            $scope.subjectlt1 = ""
            $scope.studentlist = false;
            $state.reload();
        }

        $scope.get_cls_sections = function (cls_id) {
            

            if ($scope.asmaY_Id != "" && $scope.asmaY_Id != undefined && $scope.asmaY_Id != null) {
                //$scope.selectedclasses = [];
                ////$scope.ttcC_Id = employee;

                //angular.forEach($scope.class_list, function (itm) {
                //    if (itm.asmcL_Id1 == true) {
                //        $scope.selectedclasses.push(itm);
                //    }
                //})

                var data = {
                    "ASMAY_Id": $scope.asmaY_Id,
                    "ASMCL_Id": $scope.asmcL_Id,
                    //clssids: $scope.selectedclasses,
                }

                // apiService.getURI("CategoryClassMapp/getdetails", pageid).

                apiService.create("PromotionCalculation/get_cls_sections", data).
                then(function (promise) {


                    $scope.seclist = promise.seclist;
                    $scope.asmS_Id = "";
                   // $scope.emE_Id = "";
                   // $scope.exsplt = [];
                    // $scope.exsplt = promise.exmstdlist;


                    // if ((promise.seclist == null || promise.seclist == "") && (promise.exmstdlist == null || promise.exmstdlist == "")) {
                    //     swal("Sections And Exams are Not Mapped To Selected Class!!!");
                    // }

                    //else 
                    if (promise.seclist == null || promise.seclist == "") {
                        swal("Sections are Not Mapped To Selected Class!!!");
                    }
                    //else if (promise.exmstdlist == null || promise.exmstdlist == "") {
                    //    swal("Exams are Not Mapped To Selected Class !!!");
                    //}

                })
            }
            else {

                swal("Please Select Academic Year  First !!!");
                $scope.asmcL_Id = "";

            }
        }
        $scope.interacted = function (field) {

            return $scope.submitted;
        };

        $scope.get_classes = function (ASMAY_Id) {
            var data = {
                "ASMAY_Id": ASMAY_Id
            }
            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }
            apiService.create("PromotionCalculation/get_classes", data).
       then(function (promise) {
           $scope.clslist = promise.classlist;
           $scope.asmcL_Id = "";
           $scope.asmS_Id = "";
          // $scope.emE_Id = "";
           $scope.seclist = [];
          // $scope.exsplt = [];
           if (promise.classlist == null || promise.classlist == "") {
               swal("Classes are Not Mapped To Selected Academic Year!!!");
           }

       })
        };

        //$scope.get_exams = function (ASMS_Id, ASMCL_Id, ASMAY_Id) {
        //    if (ASMAY_Id != null && ASMAY_Id != undefined && ASMCL_Id != null && ASMCL_Id != undefined) {
        //        var data = {
        //            "ASMS_Id": ASMS_Id,
        //            "ASMCL_Id": ASMCL_Id,
        //            "ASMAY_Id": ASMAY_Id
        //        }
        //        var config = {
        //            headers: {
        //                'Content-Type': 'application/json;'
        //            }
        //        }
        //        apiService.create("PromotionCalculation/get_exams", data).
        //   then(function (promise) {

        //       $scope.emE_Id = "";
        //       $scope.exsplt = promise.exmstdlist;
        //       if (promise.exmstdlist == null || promise.exmstdlist == "") {
        //           swal("Exams are Not Mapped To Selected Class And Section!!!");
        //       }
        //   })
        //    }
        //    else {

        //        swal("Please Select Academic Year  And Class First !!!");
        //        $scope.asmS_Id = "";

        //    }

        //};
        $scope.clear = function () {
           // $state.reload();
            //  $scope.BindData();
            $scope.asmaY_Id = "";
            $scope.asmcL_Id = "";
            $scope.asmS_Id = "";
          //  $scope.emE_Id = "";
            $scope.clslist = [];
            $scope.seclist = [];
           // $scope.exsplt = [];
            $scope.submitted = false;
            $scope.myForm.$setPristine();
            $scope.myForm.$setUntouched();
        };
    }

})();