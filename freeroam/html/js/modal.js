$('.ui.modal')
.modal({
  closable  : false,
  onDeny    : function(){
    return false;
  },
  onApprove : function() {
    resourceCall('modalCEF.destroy');
  }
})
.modal('show')
