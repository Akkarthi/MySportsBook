package md5167cf0c1fa468a34aa4f067b9e4d8de4;


public class Helper
	extends android.app.Activity
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"";
		mono.android.Runtime.register ("MySportsBook.Helper, MySportsBook, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", Helper.class, __md_methods);
	}


	public Helper () throws java.lang.Throwable
	{
		super ();
		if (getClass () == Helper.class)
			mono.android.TypeManager.Activate ("MySportsBook.Helper, MySportsBook, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "", this, new java.lang.Object[] {  });
	}

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
