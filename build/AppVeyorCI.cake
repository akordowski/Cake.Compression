#load "./Precondition.cake"

public class AppVeyorCI
{
    private ICakeContext context;
    private IAppVeyorProvider appVeyor;

    public AppVeyorCI(ICakeContext context)
    {
        Precondition.IsNotNull(context, nameof(context));
        
        this.context = context;
        this.appVeyor = context.BuildSystem().AppVeyor;
    }

    public static AppVeyorCI GetInstance(ICakeContext context)
    {
        Precondition.IsNotNull(context, nameof(context));
        
        return new AppVeyorCI(context);
    }

    public void UploadArtifact(FilePath file)
    {
        Precondition.IsNotNull(file, nameof(file));
        
        appVeyor.UploadArtifact(file);
    }

    public void UploadArtifacts(FilePathCollection files)
    {
        Precondition.IsNotNull(files, nameof(files));
        
        foreach(var file in files)
        {
            UploadArtifact(file);
        }
    }
}