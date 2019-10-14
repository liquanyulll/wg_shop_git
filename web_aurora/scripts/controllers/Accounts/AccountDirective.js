

var AccountDirectivesApp = angular.module('AccountDirectivesApp', ['AccountServiceApp']);

//当光标离开用户名输入框时，触发该校验，校验内容：1.是否手机号码 2.是否已经在系统中出现过
AccountDirectivesApp.directive('isExists', ['$http', 'AccountService', function ($http, AccountService) {
    return {
        restrict: 'A',
        require: 'ngModel',
        link: function (scope, ele, attrs, c) {
            scope.$watch(attrs.ngModel, function (n) {
                if (!n) {
                    c.$setValidity('exists', true);
                    return;
                }
                AccountService.isUserNameExists(n).then(function (data) {
                    c.$setValidity('exists', data.data == 'Y' ? false : true);
                }).catch(function (data) {
                    c.$setValidity('exists', false);
                });
            });
        }
    }
}]);

AccountDirectivesApp.directive('isVerfycodeok', ['$http', 'AccountService', function ($http, AccountService) {
    return {
        restrict: 'A',
        require: 'ngModel',
        link: function (scope, ele, attrs, c) {
            scope.checkVerfycode = function (n) {
                if (!n) {
                    c.$setValidity('verfycodeok', true);
                    return;
                }
                AccountService.isVerfyCodeOK(n, attrs.codedes).then(function (data) {
                    c.$setValidity('verfycodeok', data.data == 'Y' ? true : false);
                }).catch(function (data) {
                    c.$setValidity('verfycodeok', false);
                });
            };
            scope.$watch(attrs.ngModel, function (n) {
                scope.checkVerfycode(n);
            });
        }
    }
}]);

AccountDirectivesApp.directive('isSmscodeok', ['$http', 'AccountService', function ($http, AccountService) {
    return {
        restrict: 'A',
        require: 'ngModel',
        link: function (scope, ele, attrs, c) {
            scope.$watch(attrs.ngModel, function (n) {
                if (!n) {
                    c.$setValidity('smscodeok', true);
                    return;
                }
                var mobile = attrs.mobile;
                var smsType = attrs.smstype;
                AccountService.isSMSCodeOK(n, mobile, smsType).then(function (data) {
                    c.$setValidity('smscodeok', data.data == 'Y' ? true : false);
                }).catch(function (data) {
                    c.$setValidity('smscodeok', false);
                });
            });
        }
    }
}]);

AccountDirectivesApp.directive('againPass', ['$http', 'AccountService', function ($http, AccountService) {
    return {
        restrict: 'A',
        require: 'ngModel',
        link: function (scope, ele, attrs, c) {
            scope.checkAgainPass = function (n) {
                if (!n) {
                    c.$setValidity('passok', true);
                    return;
                }
                if (scope.registerInfo.Password == n) {
                    c.$setValidity('passok', true);
                } else {
                    c.$setValidity('passok', false);
                }
            }

            scope.$watch(attrs.ngModel, function (n) {
                scope.checkAgainPass(n);
            });

        }
    }
}]);

AccountDirectivesApp.directive('againPassreset', ['$http', 'AccountService', function ($http, AccountService) {
    return {
        restrict: 'A',
        require: 'ngModel',
        link: function (scope, ele, attrs, c) {
            scope.checkAgainPass = function (n) {
                if (!n) {
                    c.$setValidity('passok', true);
                    return;
                }
                if (scope.passwordInfo.Password == n) {
                    c.$setValidity('passok', true);
                } else {
                    c.$setValidity('passok', false);
                }
            }

            scope.$watch(attrs.ngModel, function (n) {
                scope.checkAgainPass(n);
            });
        }
    }
}]);