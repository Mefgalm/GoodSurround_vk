angular.module('GoodSurround').controller('SearchUserController', ['$scope', '$window', 'Users',
function ($scope, $window, Users) {
    $scope.searchString = '';

    $scope.searchStringOnChange = function () {
        Users.searchUser($scope.searchString)
            .then(function (response) {
                console.log(response);
        });
    }
}]);
