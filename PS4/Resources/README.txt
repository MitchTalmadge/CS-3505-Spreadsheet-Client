Jiahui Chen
u0980890
10/3/2017

I did not reach 100% code coverage in tests because the class voted to keep the Exception throws/parameter checks
in the helper methods of SetContentsOfCell, and these throws never occur because all checks and throws occur from the public SetContentsOfCell method.