package md5cd696db7dbef1e47e5beb4afb6d3b926;


public class GridBatchAdpater_BatchClickListener
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
		mono.android.Runtime.register ("MySportsBook.GridBatchAdpater+BatchClickListener, MySportsBook", GridBatchAdpater_BatchClickListener.class, __md_methods);
	}


	public GridBatchAdpater_BatchClickListener ()
	{
		super ();
		if (getClass () == GridBatchAdpater_BatchClickListener.class)
			mono.android.TypeManager.Activate ("MySportsBook.GridBatchAdpater+BatchClickListener, MySportsBook", "", this, new java.lang.Object[] {  });
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
