(function ($) {  
    $.fn.scrolltools = function ( op ) {
        var defaultOp = {
            showToTop               : true,
            showToReply             : false,
            showToList              : false,
            showToBottom            : true,
            alwaysShowToTop         : true,
            alwaysShowToBottom      : true,
            hiddenTopIconHeight     : 100,
            hiddenBottomIconHeight  : 100,
            replayFn                : function(){ alert('����ظ�'); },
            listFn                  : function(){ alert('�����б�'); } 
        };
        
        op = $.extend(defaultOp, op);
    
        var $outDiv   = $(this);                    //��Χdiv
        var $toTop    = $outDiv.find(".toTop");     //���ض���
        var $toReply  = $outDiv.find(".toReply");   //���ٻظ�
        var $toList   = $outDiv.find(".toList");    //�����б�
        var $toBottom = $outDiv.find(".toBottom");  //���صײ�
        
        //������Ϊ����ʾ��Ԫ�ظ�����
        $toTop.toggle(op.showToTop);
        $toReply.toggle(op.showToReply);
        $toList.toggle(op.showToList);
        $toBottom.toggle(op.showToBottom);
        
        //�����λ��������ҳ���в�
        $outDiv.css( {"top" : ($(window).height())/2  });
        $(window).resize(function(){
            $outDiv.css( {"top" : ($(window).height())/2  });
        });
        
        //��� [����ͼ��] ��Ҫ��ʾ��������Ҫһֱ��ʾ ( �붥�������Сʱ��ʧ )
        if(op.showToTop===true && op.alwaysShowToTop===false){
            $(window).scroll(function () {
                $toTop.toggle( $(window).scrollTop() > op.hiddenTopIconHeight );
            });
        }
        //��� [����ͼ��] ��Ҫ��ʾ��������Ҫһֱ��ʾ ( ��ײ������Сʱ��ʧ )
        if(op.showToBottom===true && op.alwaysShowToBottom===false){
            $(window).scroll(function () {
                $toBottom.toggle( op.hiddenBottomIconHeight + $(window).scrollTop() < $(document).height() - $(window).height() );
            });
        }
        // [����ͼ��] ��Ҫ��ʾ, Ϊ�䶨���¼�
        if(op.showToTop===true){
            $toTop.click(function(){
                window.scroll(0,0);
            });
        }
        // [����ͼ��] ��Ҫ��ʾ, Ϊ�䶨���¼�
        if(op.showToBottom===true){
            $toBottom.click(function(){
                window.scroll(0,$(document).height() - $(window).height());
            });
        }
        // [���ٻظ�] ��Ҫ��ʾ, Ϊ�䶨���¼�
        if(op.showToReply===true && typeof(op.replayFn)!="undefined"){
            $toReply.click(function(){
                op.replayFn();
            });
        }
        // [�����б�] ��Ҫ��ʾ, Ϊ�䶨���¼�
        if(op.showToList===true && typeof(op.listFn)!="undefined"){
            $toList.click(function(){
                op.listFn();
            });
        }
    }  
})(jQuery);