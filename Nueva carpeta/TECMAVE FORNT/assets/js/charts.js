
(function(){
  const primary = getComputedStyle(document.documentElement).getPropertyValue('--primary').trim() || '#2563eb';
  const text = getComputedStyle(document.documentElement).getPropertyValue('--text').trim() || '#e6e9f5';
  const grid = 'rgba(255,255,255,0.08)';
  function ctx(id){return document.getElementById(id)?.getContext('2d')}
  const c1=ctx('chartIngresos'); if(c1) new Chart(c1,{type:'line',data:{labels:['Ene','Feb','Mar','Abr','May','Jun','Jul','Ago','Sep','Oct','Nov','Dic'],datasets:[{label:'Ingresos (₡)',data:[900,1100,980,1200,1250,1420,1350,1500,1470,1600,1720,1810],borderColor:primary,backgroundColor:'rgba(14,165,233,.20)',tension:.35,fill:true,pointRadius:3}]},options:{responsive:true,maintainAspectRatio:false,plugins:{legend:{labels:{color:text}}},scales:{x:{grid:{color:grid},ticks:{color:'#a8b3c7'}},y:{grid:{color:grid},ticks:{color:'#a8b3c7'}}}}});
  const c2=ctx('chartCitas'); if(c2) new Chart(c2,{type:'bar',data:{labels:['S1','S2','S3','S4','S5','S6','S7','S8'],datasets:[{label:'Citas',data:[22,18,25,27,24,29,31,28],backgroundColor:'rgba(14,165,233,.6)',borderColor:primary,borderWidth:1,borderRadius:8}]},options:{responsive:true,maintainAspectRatio:false,plugins:{legend:{labels:{color:text}}},scales:{x:{grid:{color:grid},ticks:{color:'#a8b3c7'}},y:{grid:{color:grid},ticks:{color:'#a8b3c7'},beginAtZero:true}}}});
  const c3=ctx('chartServicios'); if(c3) new Chart(c3,{type:'doughnut',data:{labels:['Aceite','Alineado','Balanceo','Frenos','Diag.'],datasets:[{data:[35,22,14,18,11],backgroundColor:['rgba(14,165,233,.8)','rgba(245,158,11,.8)','rgba(34,197,94,.8)','rgba(239,68,68,.8)','rgba(148,163,184,.8)'],borderColor:'rgba(255,255,255,.08)',borderWidth:1}]},options:{responsive:true,maintainAspectRatio:false,plugins:{legend:{position:'bottom',labels:{color:text}}},cutout:'60%'}});
})();
