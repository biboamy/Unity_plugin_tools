#import <UIKit/UIKit.h>
#import <AVFoundation/AVFoundation.h>
#import <AssetsLibrary/AssetsLibrary.h>

@interface advSDK : UIViewController<UINavigationControllerDelegate, UIImagePickerControllerDelegate>
{
    NSString* callbackName;
}
@end

extern void UnitySendMessage(const char *, const char *, const char *);

@implementation advSDK

-(void) setName:(const char*) _name
{
    callbackName = [NSString stringWithFormat:@"%s", _name];
}

-(void)imagePickerController:(UIImagePickerController*)picker didFinishPickingMediaWithInfo:(NSDictionary*)info
{
    NSURL* localUrl = (NSURL *)[info valueForKey:UIImagePickerControllerReferenceURL];
    const char *charPath = [localUrl.absoluteString UTF8String];
    const char *callback = [callbackName UTF8String];
    
    UnitySendMessage(callback, "returnImagePath", charPath);
    
    [picker dismissViewControllerAnimated:YES completion:nil];
}

@end

static advSDK* myDelegate = NULL;

extern "C"
{
    void _albumPicker(const char * name);
    void _shareImage(const char * path);
    int GallerySave(char* path, const char * name);
}

void _albumPicker(const char *name)
{
    if(myDelegate == NULL) myDelegate = [[advSDK alloc] init];
    
    UIImagePickerController *picker = [[UIImagePickerController alloc] init];
    picker.delegate = myDelegate;
    picker.allowsEditing = YES;
    
    picker.sourceType = UIImagePickerControllerSourceTypeSavedPhotosAlbum;
    
    [myDelegate setName: name];
    [UnityGetGLViewController() presentViewController:picker animated:YES completion:nil];
}

void _shareImage(const char * path)
{
    UIImage *myImage = [UIImage imageWithContentsOfFile:[NSString stringWithFormat:@"%s", path]];
    NSArray *activityItems = @[myImage];
    UIActivityViewController *activityViewController = [[UIActivityViewController alloc] initWithActivityItems:activityItems applicationActivities:nil];
    if (UI_USER_INTERFACE_IDIOM() == UIUserInterfaceIdiomPad) {
        activityViewController.popoverPresentationController.sourceView = UnityGetGLViewController().view;
        activityViewController.popoverPresentationController.sourceRect = CGRectMake(UnityGetGLViewController().view.bounds.size.width/2, UnityGetGLViewController().view.bounds.size.height/4, 0, 0);
    }
    
    [UnityGetGLViewController() presentViewController:activityViewController animated:YES completion:nil];
}


int GallerySave(char* path, const char * name)
{
    NSString *imagePath = [NSString stringWithUTF8String:path];
    
    ALAssetsLibrary *lib = [[ALAssetsLibrary alloc] init];
    [lib enumerateGroupsWithTypes:ALAssetsGroupSavedPhotos usingBlock:^(ALAssetsGroup *group, BOOL *stop) {
    }  failureBlock:^(NSError *error) { }];
    
    ALAuthorizationStatus status = [ALAssetsLibrary authorizationStatus];
    
    if (status == ALAuthorizationStatusRestricted || status == ALAuthorizationStatusDenied)
    {
        return 2;
    }
    
    if(![[NSFileManager defaultManager] fileExistsAtPath:imagePath])
    {
        return 0;
    }
    
    UIImage *image = [UIImage imageWithContentsOfFile:imagePath];
    __block const char *url;
    
    if(image)
    {
        [lib writeImageToSavedPhotosAlbum:image.CGImage orientation:ALAssetOrientationUp completionBlock:^(NSURL *assetURL, NSError *error)
         {
             url = [assetURL.absoluteString UTF8String];
             NSLog(@"%s", url);
         }];
        return 1;
        
        UnitySendMessage(name, "galleryPath", url);
    }
    
    return 0;
}



