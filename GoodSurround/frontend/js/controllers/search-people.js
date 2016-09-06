angular.module('GoodSurround').controller('PeopleSearchController', [ '$uibModalInstance', '$scope', 'Users',
function ($uibModalInstance, $scope, Users) {
    $scope.ok = function () {
        $uibModalInstance.close('ok');
    };

    $scope.searchQuery = '';
    $scope.displayResultsThreshold = 3;
    $scope.searchResults = [];

    $scope.$watch('searchQuery', function (newValue, oldValue) {
        console.log(newValue, oldValue);
        Users.find(newValue).then(function (results) {
            // todo: implement infinite scroll
            $scope.searchResults = results;
            $scope.$apply();
        });
    });
}]);
