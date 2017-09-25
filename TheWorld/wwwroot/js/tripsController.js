(function () {

    "use strict";

    // getting the existing module
    angular.module("app-trips")
        .controller("tripsController", tripsController);

    function tripsController($http) {

        var vm = this;

        vm.trips = [];

        vm.newTrip = {};

        vm.errorMessage = "";
        vm.isBusy = true;

        $http.get("/api/trips")
            .then(
                // success
                function (response) {
                    angular.copy(response.data, vm.trips);
                },
                // failure
                function (error) {
                    vm.errorMessage = "Failed to load data: " + error;
                }
            )
            .finally(function() {
                vm.isBusy = false;
            });

        vm.addTrip = function() {
            vm.isBusy = true;
            vm.errorMessage = "";

            $http.post("/api/trips", vm.newTrip)
                .then(
                    // success
                    function (response) {
                        vm.trips.push(response.data);
                        vm.newTrip = {};
                    },
                    // failure
                    function (error) {
                        vm.errorMessage = "Failed to save new trip";
                    }
                )
                .finally(function () {
                    vm.isBusy = false;
                });
        };
    };

})();