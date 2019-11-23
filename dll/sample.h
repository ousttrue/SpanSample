#pragma once

#if _MSC_VER
#ifdef SAMPLE_BUILD
#define SAMPLE_EXPORT extern "C" __declspec(dllexport)
#else
#define SAMPLE_EXPORT extern "C" __declspec(dllimport)
#endif
#else
#define SAMPLE_EXPORT extern "C"
#endif

struct Sample;

SAMPLE_EXPORT void SAMPLE_NumberSequence(unsigned char *bytes, int length);

SAMPLE_EXPORT Sample *SAMPLE_Create();
SAMPLE_EXPORT void SAMPLE_Print(Sample *p);
SAMPLE_EXPORT void SAMPLE_Destroy(Sample *p);
