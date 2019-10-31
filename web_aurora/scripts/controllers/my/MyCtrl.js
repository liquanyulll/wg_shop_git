app.controller('MyCtrl', function ($scope, $http, $window, $location, $state , ModalService) {
    $scope.reloadUser = function () {
        $http.get(GetUrl('api/Account/CurrentUser')).then(function (res) {
            $scope.user = res.data;
        }).catch(function (resp) {
            console.log(resp);
        });
    }
    $scope.reloadUser();
    $scope.$on("myctrlreloadUser", function (event, data) {
        $scope.reloadUser();
    });

    $state.go("index.my.money");
});

app.controller('MyMoneyCtrl', function ($scope, $http, $window, $location, ModalService) {
	$scope.user = {};

    $scope.GoSave = function () {
		if (!$scope.moneykey || $scope.moneykey.length < 10 || $scope.moneykey.length > 20) {
			ModalService.Message("请输入正确的卡密", "提示");
		} else {
			$http.post(GetUrl('api/MoneyKey/UsedMk?mk=' + $scope.moneykey)).then(function (res) {
                $scope.$emit("myctrlreloadUser", "reload");
				$scope.moneykey = "";
			}).catch(function (resp) {
				console.log(resp);
			});
		}
	}
});

app.controller('MyOrderCtrl', function ($scope, $http, $window, $location, ModalService) {

	var init = function () {
		$scope.InfoList = [];
		$scope.queryVar = {};
		/* 配置分页基本参数 */
		$scope.paginationConf = {
			currentPage: 1,
			itemsPerPage: 20
		};
		$scope.paginationConf.perPageOptions = [20];
	}
	init();

	$scope.Search = function () {
		$scope.queryVar.PageIndex = $scope.paginationConf.currentPage;
		$scope.queryVar.PageSize = $scope.paginationConf.itemsPerPage;
		//$scope.queryVar.TypeId = $stateParams.tpid;

		$http.post(GetUrl('api/Order/Search'), $scope.queryVar).then(function (resp) {
			var qRtn = resp.data;
			$scope.InfoList = qRtn.contentList
			if ($scope.InfoList != null && $scope.InfoList.length > 0) {
				$scope.paginationConf.totalItems = qRtn.totalItems;
			} else {
				$scope.paginationConf.totalItems = 1;
			}
		}).catch(function (resp) {
			console.log(resp);
		});
	};
	$scope.Search();
});