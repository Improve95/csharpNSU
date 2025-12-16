using Service.service.impl;

namespace Service.objects.philosopher;

public class Aristotel(ForkFactory forkFactory, Strategy strategy)
    : Philosopher("Аристотель", forkFactory, strategy);