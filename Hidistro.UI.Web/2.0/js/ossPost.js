
function oss(file){
		
	
	var res=$.ajax({ url:'/wechat/oss',data:'',type:'get',async: false,success:function(msg){}});
		
	if(res.status==200){
		
		var request=eval("("+res.responseText+")");
		var formData = new FormData();
		formData.append("key",request.key);
		formData.append("OSSAccessKeyId",request.OSSAccessKeyId);
		formData.append("policy",request.policy);
		formData.append("Signature",request.Signature);
		formData.append("file",file);
		
		var result=$.ajax({ url : request.url,type : 'POST', data : formData,crossDomain: true,contentType: false, processData: false,async: false,success : function(msg) { }});
		
		if(result.status==204 || result.status==200){
			
			back=new Array();
			back.status=200;
			back.img=request.key;
			return back;
			
			
		}
		
		
		
		
	}

}

