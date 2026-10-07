
(function () {
    'use strict';
    angular
        .module('app')
        .controller('INV_PO_ToSupplierController', INV_PO_ToSupplierController);
    INV_PO_ToSupplierController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache', '$compile']
    function INV_PO_ToSupplierController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache,$compile) {

        var date = new Date();
        $scope.invmpI_Doc_Date = date;
        var paginationformasters;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
        }
        else {
            paginationformasters = 0
        }

        $scope.usrname = localStorage.getItem('username');
        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        if (admfigsettings.length > 0) {
            var logopath = admfigsettings[0].asC_Logo_Path;
        }
        $scope.imgname = logopath;
        
        var configsettings = JSON.parse(localStorage.getItem("feeconfigsettings"));

        $scope.attachmentflag = true;
        //====================================== Page Load
        $scope.loaddata = function () {
            $scope.currentPage = 1;
            $scope.itemsPerPage = paginationformasters;
            $scope.search = "";
            var pageid = 2;
            apiService.getURI("INV_PO_ToSupplier/getloaddata", pageid).
                then(function (promise) {
                    $scope.get_piNo = promise.get_piNo;
                    $scope.templatelist = promise.templatelist;
                    $scope.get_supplier = promise.get_supplier;
                    $scope.gridlist = promise.gridlist;
                })
        };


        $scope.printData = function () {
            var innerContents = "";
            innerContents = document.getElementById("test").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/baldwin/CumulativeReportPdf.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
                '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
            popupWinindow.document.close();
        };



        //===================================== VIEW TEMPLATE
        $scope.columdetails = [];
        $scope.viewtemplate = function () {
            debugger
            $scope.columdetails = [];
            angular.forEach($scope.columnlist, function (dd) {

                if (dd.checkedvalue === true) {
                    $scope.columdetails.push(dd);
                }

            });


            var data = {
                "INVMPO_Id": $scope.obj.invmpO_Id.invmpO_Id,
                "ISES_Id": $scope.ISES_Id,
                "columnids": $scope.columdetails

            }
            apiService.create("INV_PO_ToSupplier/viewtemplatedetails", data).
                then(function (promise) {

                    if (promise.mailmsg !== null) {
                        $scope.htmldata = promise.mailmsg;

                        var e1 = angular.element(document.getElementById("test"));
                        $compile(e1.html($scope.htmldata))(($scope));
                        $("#myModal").modal('show');
                    }
                   
                  



                });
           
        }
        //===================================== VIEW TEMPLATE





        $scope.ttdisabled = false;
        //===================================== PI Change
        $scope.onpichange = function (itemid) {
            $scope.ttdisabled = false;
            $scope.get_indentDetail = "";
            var item_id = itemid.invmpO_Id;
            var data = {
                "INVMPO_Id": item_id
            }
            apiService.create("INV_PO_ToSupplier/getpiDetail", data).
                then(function (promise) {
                    $scope.get_pidetails = promise.get_pidetails;
                    var prQty = 0;

                    if (promise.get_supplier != null && promise.get_supplier.length > 0) {
                        $scope.supname = promise.get_supplier[0].invmS_SupplierName;
                        $scope.conperson = promise.get_supplier[0].invmS_SupplierConatctPerson;
                        $scope.conmobile = promise.get_supplier[0].invmS_SupplierConatctNo;
                        $scope.conemail = promise.get_supplier[0].invmS_EmailId;
                    }



                    if (promise.get_pidetails != null && promise.get_pidetails.length > 0) {

                        $scope.refno = promise.get_pidetails[0].invmpO_ReferenceNo;
                            $scope.podate = promise.get_pidetails[0].invmpO_PODate;
                        $scope.poamount = promise.get_pidetails[0].invmpO_TotAmount;
                        $scope.poremarks = promise.get_pidetails[0].invtpO_Remarks
                        $scope.templatename = promise.get_pidetails[0].invmpO_POTemplate

                        debugger;
                        angular.forEach($scope.templatelist, function (rr) {
                            if (rr.iseS_Template_Name == $scope.templatename) {
                                $scope.ISES_Id = rr.iseS_Id;
                                $scope.ttdisabled = true;
                            }

                        })

                    angular.forEach($scope.get_pidetails, function (aq) {
                        prQty += parseFloat(aq.invtpO_POQty);
                    })
                    if (prQty == "0") {
                        $scope.pirQty = false;
                    }
                    else {
                        $scope.pirQty = true;
                    }
                    }
                })
        }
        //===================================== Radio Chang e
        $scope.onradiochange = function () {
            $scope.get_supplier = "";
            $scope.invpitS_SupplierName = "";
            $scope.invpitS_ContactNo = "";
            $scope.invpitS_EmailId = "";
            $scope.loaddata();
        }
        //===================================== Supplier Grid Select All
        $scope.toggleAll = function () {
            var selck1 = 0;
            angular.forEach($scope.get_supplier, function (subj) {
                subj.xyz = $scope.all;
                selck1 += 1;
            })
            $scope.totsel = selck1;
        };

        $scope.optionToggled = function () {
            var selck = 0;
            $scope.all = $scope.get_supplier.every(function (itm) {
                return itm.xyz;
            });

            angular.forEach($scope.get_supplier, function (sup) {
                if (sup.xyz) {
                    selck += 1;
                }
            })
            $scope.totsel = selck;
        };
        //===================================== SAVE DATA

        $scope.columnlist = [];

        $scope.columnlist.push({ ID: 1, coulmn: 'ITEMNAME', title: 'ITEM NAME', checkedvalue: true });
        $scope.columnlist.push({ ID: 7, coulmn: 'REMARKS', title: 'REMARKS', checkedvalue: false });
        $scope.columnlist.push({ ID: 2, coulmn: 'QUANTITY', title: 'QUANTITY',checkedvalue: true});
        $scope.columnlist.push({ ID: 3, coulmn: 'PRICE/UNIT', title: 'PRICE/UNIT',checkedvalue: true});
        $scope.columnlist.push({ ID: 4, coulmn: 'AMOUNT', title: 'AMOUNT',checkedvalue: true });
        $scope.columnlist.push({ ID: 5, coulmn: 'TAX', title: 'TAX', checkedvalue: true });
        $scope.columnlist.push({ ID: 6, coulmn: 'TOTALAMOUNT', title:'TOTAL AMOUNT', checkedvalue: true});
       


        $scope.savedata = function () {
            $scope.submitted = true;
            if ($scope.myForm.$valid) {

                $scope.supplierArray = [];
                angular.forEach($scope.get_supplier, function (sp) {
                    if (sp.xyz) {
                        $scope.supplierArray.push({ invmS_Id: sp.invmS_Id, invpitS_SupplierName: sp.invmS_SupplierName, invpitS_ContactNo: sp.invmS_SupplierConatctNo, invpitS_EmailId: sp.invmS_EmailId });
                    }
                })
                var data = {
                    "INVMPI_Id": $scope.obj.invmpI_Id.invmpI_Id,
                    "supplierArray": $scope.supplierArray,
                    "INVPITS_Id": $scope.invpitS_Id,
                }
                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                }
                apiService.create("INV_PO_ToSupplier/savedetails", data).then(function (promise) {

                    if (promise.returnval == true) {
                        if (promise.invpitS_Id == 0 || promise.invpitS_Id < 0) {
                            swal('Message Sent Successfully');
                        }
                        else if (promise.message == "SMS") {
                            swal("SMS Send Successfully..!!");
                        }
                        else if (promise.message == "Email") {
                            swal("Email Send Successfully..!!");
                        }
                    }
                    else {
                        if (promise.invpitS_Id == 0 || promise.invpitS_Id < 0) {
                            swal('Failed to Send, please contact administrator');
                        }
                    }
                    $state.reload();
                })
            }
            else {
                $scope.submitted = true;
            }
        };

        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        $scope.cancel = function () {
            $state.reload();
        }



        $scope.onview = function (filepath, filename) {
            debugger;
            var imagedownload = filepath;
            $scope.content = "";
            var fileURL = "";
            var file = "";
            $http.get(imagedownload, { responseType: 'arraybuffer' })
                .success(function (response) {
                    file = new Blob([(response)], { type: 'application/pdf' });
                    fileURL = URL.createObjectURL(file);
                    //  $scope.content = $sce.trustAsResourceUrl(fileURL);
                    var pdfId = document.getElementById("pdfId");
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


        $scope.sortBy = function (propertyName) {
            $scope.reverse = ($scope.propertyName === propertyName) ? !$scope.reverse : false;
            $scope.propertyName = propertyName;
        };
        $scope.searchValue = '';


        $scope.sendmail = function () {
            
            var mgs = "";
            var confirmmgs = "";
            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }
            
            swal({
                title: "Are you sure",
                text: "Do you want to Send a Purchase Order Details ??????",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, Send it!",
                cancelButtonText: "Cancel",
                closeOnConfirm: false,
                closeOnCancel: false
            },
                function (isConfirm) {
                    if (isConfirm) {
                        var Template = document.getElementById('test').innerHTML;
                        var data = {
                            "INVMPO_Id": $scope.obj.invmpO_Id.invmpO_Id,
                            "ISES_Id": $scope.ISES_Id,
                            "columnids": $scope.columdetails,
                            "mailmsg": Template,
                            "INVPITS_EmailId": $scope.conemail,
                            "INVTPOSUP_ContactNo": $scope.conmobile,
                            "INVTPOSUP_SupplierName": $scope.supname,
                            "attachmentflag": $scope.attachmentflag

                        }
                        apiService.create("INV_PO_ToSupplier/sendmail", data).
                            then(function (promise) {
                                if (promise.returnval == true) {
                                    swal('Purchase Order Sent Successfully');
                                }
                                else {
                                    swal('Failed To Send a Purchase Order???');
                                }
                            })
                    }
                    else {
                        swal("Cancelled");
                    }
                    $state.reload();
                });









          

            
          


        }


       
    }
})();