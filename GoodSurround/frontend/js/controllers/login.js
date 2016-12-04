angular.module('GoodSurround').controller('LoginController', ['$scope', '$rootScope', '$window',
function ($scope, $rootScope, $window) {
    $scope.logIn = function () {
        $window.location.href = 'https://oauth.vk.com/authorize?client_id=5559326&display=page&redirect_uri=http://localhost:54690/Home/Redirect&scope=friends,audio&response_type=code&v=5.53';
    }

}]);