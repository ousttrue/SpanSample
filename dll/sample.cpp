#include "sample.h"
#include <iostream>

// impl
struct Position
{
    float x = 0;
    float y = 0;
    float z = 0;
};

struct Sample
{
    int index = 0;
    Position value;

    void print()
    {
        std::cout << index << "["
                  << value.x
                  << ", " << value.y
                  << ", " << value.z
                  << "]" << std::endl;
    }
};

SAMPLE_EXPORT void SAMPLE_NumberSequence(unsigned char *bytes, int length)
{
    for (unsigned char i = 0; i < length; ++i)
    {
        bytes[i] = i;
    }
}

SAMPLE_EXPORT Sample *SAMPLE_Create()
{
    return new Sample;
}

SAMPLE_EXPORT void SAMPLE_Print(Sample *p)
{
    if (p)
    {
        p->print();
    }
}

SAMPLE_EXPORT void SAMPLE_Destroy(Sample *p)
{
    if (p)
    {
        delete p;
    }
}
