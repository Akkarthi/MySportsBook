package md5167cf0c1fa468a34aa4f067b9e4d8de4;


public class BatchPlayer_ItemAdapter_ImageClickListener
	extends java.lang.Object
	implements
		mono.android.IGCUserPeer,
		android.view.View.OnClickListener
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_onClick:(Landroid/view/View;)V:GetOnClick_Landroid_view_View_Handler:Android.Views.View/IOnClickListenerInvoker, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null\n" +
			"";
		mono.android.Runtime.register ("MySportsBook.BatchPlayer_ItemAdapter+ImageClickListener, MySportsBook, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", BatchPlayer_ItemAdapter_ImageClickListener.class, __md_methods);
	}


	public BatchPlayer_ItemAdapter_ImageClickListener () throws java.lang.Throwable
	{
		super ();
		if (getClass () == BatchPlayer_ItemAdapter_ImageClickListener.class)
			mono.android.TypeManager.Activate ("MySportsBook.BatchPlayer_ItemAdapter+ImageClickListener, MySportsBook, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "", this, new java.lang.Object[] {  });
	}

	public BatchPlayer_ItemAdapter_ImageClickListener (int p0, android.app.Activity p1) throws java.lang.Throwable
	{
		super ();
		if (getClass () == BatchPlayer_ItemAdapter_ImageClickListener.class)
			mono.android.TypeManager.Activate ("MySportsBook.BatchPlayer_ItemAdapter+ImageClickListener, MySportsBook, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "System.Int32, mscorlib, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e:Android.App.Activity, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=84e04ff9cfb79065", this, new java.lang.Object[] { p0, p1 });
	}


	public void onClick (android.view.View p0)
	{
		n_onClick (p0);
	}

	private native void n_onClick (android.view.View p0);

	private java.util.ArrayList refList;
	public void monodroidAddReference (java.lang.Object obj)
	{
		if (refList == null)
			refList = new java.util.ArrayList ();
		refList.add (obj);
	}

	public void monodroidClearReferences ()
	{
		if (refList != null)
			refList.clear ();
	}
}
