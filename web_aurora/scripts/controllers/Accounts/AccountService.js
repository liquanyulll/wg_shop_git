var AccountServiceApp = angular.module('AccountServiceApp', []);

AccountServiceApp.service('AccountService', ['$http', function ($http) {

    this.isUserNameExists = function (n) {
        var httpService = $http({
            method: 'GET',
            url: GetUrl('/api/Account/CheckUserName?userName=' + n)
        });
        return httpService;
    };

    this.isVerfyCodeOK = function (code, codeDes) {
        var httpService = $http({
            method: 'GET',
            url: GetUrl('/api/Home/CheckVCode?code=' + code + '&codeDes=' + codeDes)
        });
        return httpService;
    };

    this.sendSMSCode = function (code, codeDes, mobile, smsType) {
        smsType = smsType || "all";
        var httpService = $http({
            method: 'GET',
            url: GetUrl('/api/Home/SendSmsVCode?VGcode=' + code + '&VGcodeDes=' + codeDes + '&targetMobile=' + mobile + '&smsType=' + smsType)
        });
        return httpService;
    };

    this.isSMSCodeOK = function (code, mobile, smsType) {
        smsType = smsType || "all";
        var httpService = $http({
            method: 'GET',
            url: GetUrl('/api/Home/CheckVCode?code=' + code + '&codeDes=&mobile=' + mobile + "&smsType=" + smsType)
        });
        return httpService;
    };

}]);