function setupBundleModal(triggerSelector) {
    $(document).on('click', triggerSelector, function () {
        const bundleName = $(this).data('name');
        const url = $(this).data('url'); // dynamic per view

        if (!url) {
            console.error('No URL provided for bundle modal.');
            return;
        }

        $('.bs-example-modal-center').modal('show');
        $('#bundleDetailLoader').show();
        $('#bundleDetailModalBody').html('');

        fetch(`${url}?name=${encodeURIComponent(bundleName)}`)
            .then(response => response.text())
            .then(html => {
                $('#bundleDetailLoader').hide();
                $('#bundleDetailModalBody').html(html);
                $('.bs-example-modal-center').modal('show');
            })
            .catch(() => {
                $('#bundleDetailLoader').hide();
                $('#bundleDetailModalBody').html('<p class="text-danger">Failed to load bundle details.</p>');
            });
    });
}
