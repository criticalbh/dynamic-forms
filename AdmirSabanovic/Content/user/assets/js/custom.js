/* 
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */


(function() {

        var dlgtrigger = document.querySelector('[data-dialog]'),
                somedialog = document.getElementById(dlgtrigger.getAttribute('data-dialog')),
                dlg = new DialogFx(somedialog);

        dlgtrigger.addEventListener('click', dlg.toggle.bind(dlg));

})();

(function() {
    
        var dlgtrigger = document.querySelector('[login-dialog]'),
                somedialog = document.getElementById(dlgtrigger.getAttribute('login-dialog')),
                dlg = new DialogFx(somedialog);

        dlgtrigger.addEventListener('click', dlg.toggle.bind(dlg));

})();

(function() {
        var formWrap = document.getElementById( 'fs-form-wrap' );

        [].slice.call( document.querySelectorAll( 'select.cs-select' ) ).forEach( function(el) {	
                new SelectFx( el, {
                        stickyPlaceholder: false,
                        onChange: function(val){
                                document.querySelector('span.cs-placeholder').style.backgroundColor = val;
                        }
                });
        } );

        new FForm( formWrap, {
                onReview : function() {
                        classie.add( document.body, 'overview' ); // for demo purposes only
                }
        } );
})();