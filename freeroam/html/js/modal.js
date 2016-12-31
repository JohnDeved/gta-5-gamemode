$('.ui.modal')
.modal({
  closable  : false,
  onDeny    : function(){
    return false;
  },
  onApprove : function() {
    modalCEF.destroy();
  }
})
.modal('show')
