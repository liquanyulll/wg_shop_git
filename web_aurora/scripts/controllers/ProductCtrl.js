app.controller('ProductCtrl', function ($scope, $http, $rootScope, $stateParams, $state, ModalService) {
	var pid = $stateParams.id;
	var invCode = $stateParams.invcode;

	console.log($stateParams);

	//邀请人
	if (invCode) {
		$http.post(GetUrl('api/Account/LogInv?invCode=' + invCode), null).then(function (resp) {
		}).catch(function (resp) {
			console.log(resp);
		});
	}
	if (!pid || isNaN(Number(pid))) {
		$state.go("index.productList");
	}

	$scope.getProductInfo = function () {
		$http.post(GetUrl('api/Product/GetById?pid=' + pid), null).then(function (resp) {
			$scope.product = resp.data;
			$scope.invUrl = location.href;
			$scope.invUrl = $scope.invUrl.substring(0, $scope.invUrl.lastIndexOf("/product/")) + "/product/" + pid + "/"
		}).catch(function (resp) {
			console.log(resp);
		});
	}

	$scope.getProductInfo();

	$scope.createInv = function () {
		$http.post(GetUrl('api/Account/CreateInvProduct?pid=' + pid), null).then(function (resp) {
			$scope.product.invInfo = resp.data;
		}).catch(function (resp) {
			console.log(resp);
		});
	}

	$scope.Pay = function (way) {
		if ($scope.product.isBuy) {
			ModalService.Message("该用户已购买/获取资源，无须重复购买/获取");
		} else {
			var msg = way == "amt" ? "购买" : "获取";

			ModalService.Confirm("确认" + msg + "?", function () {
				$http.post(GetUrl('api/Product/BuyProduct?way=' + way + '&pid=' + pid), null).then(function (resp) {
					$scope.getProductInfo();
				}).catch(function (resp) {
					console.log(resp);
				});
			});
		}
	}
});
